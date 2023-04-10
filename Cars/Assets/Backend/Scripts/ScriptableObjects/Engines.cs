using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Engine", menuName = "ScriptableObjects/Car_parts/Engine")]
public class Engines : ScriptableObject
{
    public string engineName;
    public float maxRPM;
    public float idleRPM;
    public AnimationCurve torqueCurve;
    public AnimationCurve HorsepowerCurve;
    [Range(0f, 2f)] public float engineBrakingValue = 0.9f;
}
