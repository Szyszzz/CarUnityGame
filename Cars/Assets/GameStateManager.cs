using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public int PointsGoal;
    public int FullScore;

    public bool wasGoalAchieved = false;

    public TextMeshProUGUI goalUI;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;

        goalUI.text = PointsGoal.ToString();
    }

    private void Start()
    {
        TimeManager.instance.onTimeLimitPassed += TimePassed;
    }

    private void Update()
    {
        if (FullScore < PointsGoal)
            return;

        if(!wasGoalAchieved)
        {
            PointGoalAchieved();
        }
    }

    private void PointGoalAchieved()
    {
        wasGoalAchieved = true;
    }

    private void TimePassed()
    {

    }
}
