using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCarScript : MonoBehaviour
{
    public Rigidbody CarRigidbody;
    public float MassMultiplier = 1;
    public ScoreBoard score;
    private int ScoreMultiplier = 1;

    [SerializeField] private float stockMass;
    [SerializeField] private float addedMass;
    [SerializeField] private float currentMass;
    [SerializeField] private int currentPoints;
    [SerializeField] private float massPercent;

    void Start()
    {
        stockMass = CarRigidbody.mass;
        addedMass = 0;
        currentMass = stockMass;
        currentPoints = 0;
        massPercent = currentMass / stockMass;

        score.UpdateNumbers(score.score, currentPoints.ToString());
        score.UpdateNumbers(score.multiplier, ScoreMultiplier.ToString());
        score.UpdateNumbers(score.weight, Mathf.RoundToInt(100 * massPercent).ToString() + "%");
    }

    public void PickupBox(float mass, int points)
    {
        addedMass += MassMultiplier * mass;
        currentMass = stockMass + addedMass;
        CarRigidbody.mass = currentMass;

        massPercent = currentMass / stockMass;
        currentPoints += points * ScoreMultiplier;

        ScoreMultiplier++;

        score.UpdateNumbers(score.score, currentPoints.ToString());
        score.UpdateNumbers(score.multiplier, ScoreMultiplier.ToString());
        score.UpdateNumbers(score.weight, Mathf.RoundToInt(100 * massPercent).ToString() + "%");
    }

    public int DropOffBox()
    {
        ScoreMultiplier = 1;

        currentMass = stockMass;
        addedMass = 0;
        CarRigidbody.mass = currentMass;
        massPercent = currentMass / stockMass;

        int points = currentPoints;
        currentPoints = 0;

        GameStateManager.instance.FullScore += points;

        score.UpdateNumbers(score.score, currentPoints.ToString());
        score.UpdateNumbers(score.multiplier, ScoreMultiplier.ToString());
        score.UpdateNumbers(score.weight, Mathf.RoundToInt(100 * massPercent).ToString() + "%");

        return points;
    }
}
