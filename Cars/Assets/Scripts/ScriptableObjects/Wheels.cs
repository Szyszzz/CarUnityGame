using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wheels", menuName ="ScriptableObjects/Car_parts/Wheels")]
public class Wheels : ScriptableObject
{
    public GameObject wheel;

    public GameObject LoadWheel() => wheel;
}
