using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DropoffObject : MonoBehaviour
{
    [SerializeField] private int collectedPoints;
    public PickupCarScript carPointsScript;
    public ScoreBoard score;

    // Start is called before the first frame update
    void Start()
    {
        collectedPoints = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            collectedPoints += carPointsScript.DropOffBox();
            score.UpdateNumbers(score.fullscore, collectedPoints.ToString());
        }
    }
}
