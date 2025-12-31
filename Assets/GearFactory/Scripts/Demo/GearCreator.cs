using System.Collections.Generic;
using UnityEngine;

namespace GearFactory
{
    /// <summary>
    /// Class for Unity Editor, making the prototyping
    /// new gears easier by drawing the shape of gear
    /// on scene.
    /// Sliders are restricted to some values (vide
    /// GearCreatorEditor), but they other values
    /// can be assigned as well.
    /// </summary>
    public class GearCreator : MonoBehaviour
    {
        [HideInInspector] public int gearSides = 12;
        [HideInInspector] public float gearInnerRadius = 0.75f;
        [HideInInspector] public Vector2 gearInnerControlPoint = new Vector2(0.6f, 0.6f);
        [HideInInspector] public float gearRootRadius = 1f;
        [HideInInspector] public Vector2 gearRootControlPoint = new Vector2(0.7f, 0.7f);
        [HideInInspector, Range(-0.5f, 0.5f)] public float gearRootAngleShiftMultiplier = -0.05f;
        [HideInInspector] public float gearOuterRadius = 1.2f;
        [HideInInspector] public Vector2 gearOuterControlPoint = new Vector2(1f, 1f);
        [HideInInspector, Range(-0.5f, 0.5f)] public float gearOuterAngleShiftMultiplier = 0.01f;
        [HideInInspector] public float gearTipRadius = 1.5f;
        [HideInInspector] public Vector2 gearTipControlPoint = new Vector2(1.2f, 1.2f);
        [HideInInspector, Range(-0.5f, 0.5f)] public float gearTipAngleShiftMultiplier = 0.3f;
        [HideInInspector, Range(-0.5f, 0.5f)] public float gearTeethSlant = 0;

        public float thickness = 0.1f;

        public Material material;
        public bool attachRigidbody;
        [Range(0f, 10f)] public float friction = 0.4f;
        [Range(0f, 1f)] public float bounciness = 0.1f;

        [Header("Auto Size")]
        public bool autoSize = true;
        public float toothSize = 0.1f;

        public float autoSizeBaseScale = 0.1f;

        private float pitchRadius => gearSides * autoSizeBaseScale;
        public float actualRootRadius => autoSize ? pitchRadius - AutoToothDepth * 0.6f : gearRootRadius;
        public float actualOuterRadius => autoSize ? pitchRadius + AutoToothDepth * 0.6f : gearOuterRadius;
        public float actualTipRadius => autoSize ? pitchRadius + AutoToothDepth : gearTipRadius;

        // Calculate tooth depth based on angular spacing
        private float AngleDelta => Mathf.PI * 2f / (gearSides * 2f);
        private float AutoToothDepth => AngleDelta * pitchRadius * 0.5f;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Vector2[] points = GetGearPoints();
            Vector2 pos = transform.position;

            for (int i = 0; i < points.Length; i++)
            {
                Gizmos.DrawLine(pos + points[i], pos + points[(i + 1) % points.Length]);
            }
        }

        //generate outline from gear data
        private Vector2[] GetGearPoints()
        {
            List<Vector3> vertices = new List<Vector3>();

            float angleDelta = 360f * Mathf.Deg2Rad / (gearSides * 2);
            float angleShift = angleDelta * 0.5f;

            float rootAngleShift = Mathf.Max(Mathf.Abs(angleDelta * gearRootAngleShiftMultiplier), 0.01f) * Mathf.Sign(gearRootAngleShiftMultiplier);
            float outerAngleShift = Mathf.Max(Mathf.Abs(angleDelta * gearOuterAngleShiftMultiplier), 0.01f) * Mathf.Sign(gearOuterAngleShiftMultiplier);
            float tipAngleShift = Mathf.Max(Mathf.Abs(angleDelta * gearTipAngleShiftMultiplier), 0.01f) * Mathf.Sign(gearTipAngleShiftMultiplier);
            float teethSlant = gearTeethSlant * angleDelta;

            //inner vertices
            for (int i = 0; i < gearSides * 2; i++)
            {
                vertices.Add(
                    new Vector3(Mathf.Cos(i * angleDelta + angleShift), Mathf.Sin(i * angleDelta + angleShift)) *
                    gearInnerRadius);
            }

            //root vertices
            for (int i = 0; i < gearSides * 2; i++)
            {
                float angle = i * angleDelta + angleShift + (i % 2 == 0 ? rootAngleShift : -rootAngleShift);
                vertices.Add(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * actualRootRadius);
            }

            //outer vertices
            for (int i = 0; i < gearSides; i++)
            {
                float angle = 2 * i * angleDelta + angleShift + outerAngleShift + teethSlant;
                vertices.Add(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * actualOuterRadius);
                angle = (2 * i + 1) * angleDelta + angleShift - outerAngleShift + teethSlant;
                vertices.Add(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * actualOuterRadius);
            }

            //tip vertices
            for (int i = 0; i < gearSides; i++)
            {
                float angleLeft = 2 * i * angleDelta + angleShift + tipAngleShift + teethSlant * 2;
                vertices.Add(new Vector3(Mathf.Cos(angleLeft), Mathf.Sin(angleLeft)) * actualTipRadius);

                float angleRight = (2 * i + 1) * angleDelta + angleShift - tipAngleShift + teethSlant * 2;
                vertices.Add(new Vector3(Mathf.Cos(angleRight), Mathf.Sin(angleRight)) * actualTipRadius);
            }

            Vector2[] outlinePoints = new Vector2[8 * gearSides + 2];
            for (int i = 0; i < 2 * gearSides; i++)
            {
                outlinePoints[i] = vertices[i];
            }
            outlinePoints[2 * gearSides] = vertices[0];

            int index = 2 * gearSides + 1;
            for (int i = 0; i < gearSides; i++)
            {
                outlinePoints[index++] = vertices[(2 * gearSides + i * 2) % vertices.Count];
                outlinePoints[index++] = vertices[(4 * gearSides + i * 2) % vertices.Count];
                outlinePoints[index++] = vertices[(6 * gearSides + 2 * i) % vertices.Count];
                outlinePoints[index++] = vertices[(6 * gearSides + 1 + 2 * i) % vertices.Count];
                outlinePoints[index++] = vertices[(4 * gearSides + 1 + 2 * i) % vertices.Count];
                outlinePoints[index++] = vertices[(2 * gearSides + 1 + i * 2) % vertices.Count];
            }
            outlinePoints[index] = vertices[2 * gearSides];

            return outlinePoints;
        }
    }
}
