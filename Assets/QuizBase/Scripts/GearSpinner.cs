using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GearSpinner : MonoBehaviour
{
    public List<GameObject> gears;

    public List<GameObject> attachedGears;

    public int teeth;

    public float angularSpeedDeg = 180f; // degrees per second

    public bool motor;

    public GameObject turnsText;

    public int maxTurns;

    public GameObject trailPoint;

    public float resetSeconds;

    private float resetT;

    private int turns;

    private float currentAngle;

    void Awake()
    {
        if (trailPoint != null)
        {
            trailPoint.GetComponent<TrailRenderer>().time = 360f/angularSpeedDeg + 0.25f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (resetT == 0)
        {
            if (motor)
            {
                Rotate();
            }
        } else if (Time.time >= resetT)
        {
            turns = 0;
            resetT = 0;
            currentAngle = 0;

            if (turnsText != null)
            {
                turnsText.GetComponent<TextMeshProUGUI>().SetText("0");
            }

            if (trailPoint != null)
            {
                trailPoint.GetComponent<TrailRenderer>().Clear();
            }
        }
    }

    public void Rotate()
    {
        currentAngle += angularSpeedDeg * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);

        if (maxTurns > 0 && currentAngle >= 360)
        {
            turns++;
            currentAngle %= 360;

            if (turnsText != null) {
                turnsText.GetComponent<TextMeshProUGUI>().SetText(turns.ToString());
            }

            if (turns >= maxTurns)
                resetT = Time.time + resetSeconds;
        }

        foreach (GameObject gear in attachedGears)
        {
            GearSpinner gearSpinner = gear.GetComponent<GearSpinner>();
            gearSpinner.angularSpeedDeg = angularSpeedDeg;

            gearSpinner.Rotate();
        }

        foreach (GameObject gear in gears)
        {
            GearSpinner gearSpinner = gear.GetComponent<GearSpinner>();
            float ratio = teeth / (float)gearSpinner.teeth;
            gearSpinner.angularSpeedDeg = -ratio * angularSpeedDeg;

            gearSpinner.Rotate();
        }
    }
}
