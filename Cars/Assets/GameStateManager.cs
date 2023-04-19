using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public Color GoalColor1;
    public Color GoalColor2;

    public float Timescale;

    public GameObject FinalScreen;
    public GameObject WinText;
    public GameObject LoseText;
    public GameObject Point;
    public UnityEngine.UI.Image bg;
    public GameObject Retry;
    public GameObject Exit;

    private UnityEngine.UI.Button exitB;
    private UnityEngine.UI.Button retryB;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;

        exitB = Exit.GetComponent<UnityEngine.UI.Button>();
        retryB = Retry.GetComponent<UnityEngine.UI.Button>();

        exitB.onClick.AddListener(ExitClick);
        retryB.onClick.AddListener(RetryClick);

        goalUI.color = GoalColor1;
        goalUI.text = PointsGoal.ToString();
    }

    private void Start()
    {
        WinText.SetActive(false);
        LoseText.SetActive(false);
        Point.SetActive(false);
        FinalScreen.SetActive(false);
        Exit.SetActive(false);
        Retry.SetActive(false);

        TimeManager.instance.onTimeLimitPassed += TimePassed;
    }

    private void Update()
    {
        Timescale = Time.timeScale;
        if (hasTimePassed)
        {
            FinalScreen.SetActive(true);
            Color bgcolor = bg.color;
            if (Time.timeScale <= 0.1f)
            {
                WinLose();
                Time.timeScale = 0f;
                bgcolor.a = 0.7f - (Timescale * 0.7f);
                bg.color = bgcolor;
            }
            else
            {
                bgcolor.a = 0.7f - (Timescale * 0.7f);
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
        Point.SetActive(true);
        Retry.SetActive(true);
        Exit.SetActive(true);
        finalUI.text = FullScore.ToString();

        if(wasGoalAchieved)
            WinText.SetActive(true);
        else
            LoseText.SetActive(true);
    }

    private void PointGoalAchieved()
    {
        goalUI.color = GoalColor2;
        wasGoalAchieved = true;
    }

    private void TimePassed()
    {
        hasTimePassed = true;
    }

    private void ExitClick()
    {       
        Debug.Log("Exit");

        Application.Quit();
    }

    private void RetryClick()
    {
        Debug.Log("Retry");

        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1;
    }
}
