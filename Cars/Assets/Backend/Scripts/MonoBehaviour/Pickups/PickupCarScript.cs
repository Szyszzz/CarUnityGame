using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCarScript : MonoBehaviour
{
    public Rigidbody CarRigidbody;
    public float MassMultiplier = 1;

    [SerializeField] private float stockMass;
    [SerializeField] private float addedMass;
    [SerializeField] private float currentMass;
    [SerializeField] private int currentPoints;

    void Start()
    {
        stockMass = CarRigidbody.mass;
        addedMass = 0;
        currentMass = stockMass;
        currentPoints = 0;
    }

    public void PickupBox(float mass, int points)
    {
        addedMass += MassMultiplier * mass;
        currentMass = stockMass + addedMass;
        CarRigidbody.mass = currentMass;
        currentPoints += points;
    }

    public int DropOffBox()
    {
        currentMass = stockMass;
        addedMass = 0;
        CarRigidbody.mass = currentMass;

        int points = currentPoints;
        currentPoints = 0;

        return points;
    }
}
