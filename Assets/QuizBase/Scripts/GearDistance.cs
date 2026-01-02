using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class GearDistance : MonoBehaviour
{
    public int teeth;
    public float radius;                 // Gear radius in Unity units
    
    public float angularSpeedDeg = 180f; // degrees per second
    public int turns;                    // How many turns it should do

    private Vector3 startingPosition;
    private float resetT;
    private float accumulatedTurnAngleDeg = 0f;
    private float accumulatedAngleDeg = 0f;

    private LineRenderer lineRenderer;
    private List<Vector3> linePositions = new List<Vector3>();
    private List<GameObject> points = new List<GameObject>();

    private float realRadius;

    void Start()
    {
        startingPosition = transform.position;

        realRadius = GetComponent<MeshRenderer>().bounds.extents.magnitude *0.75f;

        // Add LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 0;

        // Add first point at bottom
        AddLinePoint();
    }

    void SpawnPoint()
    {
        GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        point.transform.localScale = Vector3.one * 0.05f;
        point.transform.position = transform.position - new Vector3(0, realRadius, 0);

        points.Add(point);
    }

    void AddLinePoint()
    {
        Vector3 bottomPos = transform.position - new Vector3(0, realRadius, 0);
        linePositions.Add(bottomPos);
        lineRenderer.positionCount = linePositions.Count;
        lineRenderer.SetPositions(linePositions.ToArray());
    }

    void Update()
    {
        if (resetT == 0)
        {
            float deltaAngle = angularSpeedDeg * Time.deltaTime;
            accumulatedTurnAngleDeg += deltaAngle;
            accumulatedAngleDeg += deltaAngle;

            // Rotate gear
            transform.Rotate(new Vector3(0, 0, -deltaAngle), deltaAngle, Space.Self);

            // Move gear forward based on rotation
            float deltaAngleRad = deltaAngle * Mathf.Deg2Rad;
            float distance = deltaAngleRad * radius;
            transform.localPosition += Vector3.right * distance;

            // Add bottom point for LineRenderer
            AddLinePoint();

            // Check distance for reset
            if (accumulatedAngleDeg >= turns * 360)
            {
                resetT = Time.time + 5;
            } else if (accumulatedTurnAngleDeg >= 360f)
            {
                accumulatedTurnAngleDeg -= 360f;
                SpawnPoint();
            }
        }
        else if (Time.time >= resetT)
        {
            // Reset
            resetT = 0;
            transform.position = startingPosition;
            accumulatedTurnAngleDeg = 0;
            accumulatedAngleDeg = 0;

            foreach (var point in points)
            {
                Destroy(point);
            }
            points.Clear();

            linePositions.Clear();
            lineRenderer.positionCount = 0;
            
            AddLinePoint();
        }
    }
}
