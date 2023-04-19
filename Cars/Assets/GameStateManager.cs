using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    public int PointsGoal;
    public int FullScore;

    public bool wasGoalAchieved = false;
    public bool hasTimePassed = false;

    public TextMeshProUGUI goalUI;
    public TextMeshProUGUI finalUI;

    public Color goalColor1;
    public Color goalColor2;

    public float timescale;

    public GameObject finalScreen;
    public GameObject winText;
    public GameObject loseText;
    public GameObject point;
    public UnityEngine.UI.Image bg;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;

        goalUI.color = goalColor1;
        goalUI.text = PointsGoal.ToString();
    }

    private void Start()
    {
        winText.SetActive(false);
        loseText.SetActive(false);
        point.SetActive(false);
        finalScreen.SetActive(false);

        TimeManager.instance.onTimeLimitPassed += TimePassed;
    }

    private void Update()
    {
        timescale = Time.timeScale;
        if (hasTimePassed)
        {
            finalScreen.SetActive(true);
            Color bgcolor = bg.color;
            if (Time.timeScale <= 0.1f)
            {
                WinLose();
                Time.timeScale = 0f;
                bgcolor.a = 0.7f - (timescale * 0.7f);
                bg.color = bgcolor;
            }
            else
            {
                bgcolor.a = 0.7f - (timescale * 0.7f);
                bg.color = bgcolor;
                Time.timeScale -= (0.5f * Time.deltaTime);
            }
            return;
        }

        if (FullScore < PointsGoal)
            return;

        if(!wasGoalAchieved)
        {
            PointGoalAchieved();
        }
    }

    private void WinLose()
    {
        point.SetActive(true);
        finalUI.text = FullScore.ToString();

        if(wasGoalAchieved)
            winText.SetActive(true);
        else
            loseText.SetActive(true);
    }

    private void PointGoalAchieved()
    {
        goalUI.color = goalColor2;
        wasGoalAchieved = true;
    }

    private void TimePassed()
    {
        hasTimePassed = true;
    }
}
