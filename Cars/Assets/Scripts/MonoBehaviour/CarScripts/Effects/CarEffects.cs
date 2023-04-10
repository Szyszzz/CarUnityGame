using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider[] wheels;

    [Space(5)]
    [Header("Smoke")]
    [Range(0f, 1f)]
    public float slipLimit = 0.15f;
    public ParticleSystem smokePrefab;

    [HideInInspector]
    public ParticleSystem[] particles;

    private void InitializeParticleSystems()
    {
        particles = new ParticleSystem[wheels.Length];
        for (int i = 0; i < wheels.Length; i++)
        {
            Transform smokeTransform = wheels[i].transform;
            ParticleSystem smoke = Instantiate(smokePrefab, smokeTransform);
            smoke.Stop();
            particles[i] = smoke;
        }
    }

    private void ReleaseSmokeFromWheels()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            WheelHit hit = new WheelHit();
            wheels[i].GetGroundHit(out hit);

            if (Mathf.Abs(hit.forwardSlip) + Mathf.Abs(hit.sidewaysSlip) >= slipLimit)
            {
                if (!particles[i].isPlaying)
                {
                    particles[i].Play();
                }
            }
            else
            {
                particles[i].Stop();
            }
        }
    }


    private void Start()
    {
        InitializeParticleSystems();
    }

    void FixedUpdate()
    {
        ReleaseSmokeFromWheels();
    }
}
