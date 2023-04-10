using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public Engines engine;

    public float currentRPM;
    public float currentTorque;
    public float currentHP;

    void Start()
    {
        currentRPM = engine.idleRPM;

        currentTorque = engine.torqueCurve.Evaluate(currentRPM);
        currentHP = engine.HorsepowerCurve.Evaluate(currentRPM);
    }

    // Update is called once per frame
    void Update()
    {
        currentTorque = engine.torqueCurve.Evaluate(currentRPM);
        currentHP = engine.HorsepowerCurve.Evaluate(currentRPM);
        //currentRPM = currentHP / currentTorque * 7127;
    }
}
