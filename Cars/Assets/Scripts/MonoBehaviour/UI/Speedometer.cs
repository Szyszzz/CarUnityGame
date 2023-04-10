using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    public CarMovement1 car;
    [Range(0f, 10f)] public float speedMultiplier = 4f;

    private Rigidbody rigid;

    private void OnValidate()
    {
        rigid = car.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        string speed = ((int)(rigid.velocity.magnitude * speedMultiplier)).ToString();

        while(speed.Length < 3)
        {
            speed = "0" + speed;
        }
        gameObject.GetComponent<Text>().text = speed;
    }
}
