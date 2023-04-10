using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tachometer : MonoBehaviour
{
    public float minRot = 120f;
    public float maxRot = -120f;

    public Transform needle;

    public CarMovement1 car;

    private float wholeRot;
    private float lastRpm;

    private Quaternion NeedleRotation()
    {
        float zRot =  minRot - wholeRot * Mathf.Lerp(lastRpm ,(car.motorRPM / car.engine.maxRPM), Time.deltaTime);
        lastRpm = car.motorRPM / car.engine.maxRPM;
        return Quaternion.Euler(0f, 0f, zRot);
    }    

    private void OnValidate()
    {
        wholeRot = Mathf.Abs(minRot) + Mathf.Abs(maxRot);     
    }

    private void Update()
    {
        needle.rotation = NeedleRotation(); 
    }

}
