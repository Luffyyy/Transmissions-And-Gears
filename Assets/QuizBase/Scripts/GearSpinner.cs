using System.Collections.Generic;
using UnityEngine;

public class GearSpinner : MonoBehaviour
{
    public List<GameObject> gears;
    public int teeth;
    public Vector3 speed;

    public Vector3 currentSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (speed != Vector3.zero)
        {
            currentSpeed = speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpeed != Vector3.zero)
        {
            transform.Rotate(currentSpeed);
        }

        foreach (GameObject gear in gears)
        {
            GearSpinner gearInfo = gear.GetComponent<GearSpinner>();
            float ratio = teeth / (float)gearInfo.teeth;
            gearInfo.currentSpeed = -ratio * currentSpeed;
        }
    }
}
