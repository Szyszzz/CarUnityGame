using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public Vector2 TimeLimit;
    public float timeInSeconds;
    public event Action onTimeLimitPassed;
    public bool WasEventSent = false;
    public Vector2 RemainingTime;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        
        instance = this;
    }

    void Start()
    {
        timeInSeconds = TimeLimit.x * 60 + TimeLimit.y;

        instance.onTimeLimitPassed += TimePassed;
    }

    private void TimePassed()
    {
        Debug.Log("Time passed");
    }

    void Update()
    {
        if (WasEventSent)
            return;

        timeInSeconds -= Time.deltaTime;
        RemainingTime = RemainingTimeInMinutes();

        if (timeInSeconds <= 0)
            TimeLimitPassed();

    }

    private void TimeLimitPassed()
    {
        onTimeLimitPassed();
        WasEventSent = true;
    }

    public Vector2 RemainingTimeInMinutes()
    {
        Vector2 time = new Vector2(MathF.Floor(timeInSeconds/60), Mathf.Floor(timeInSeconds%60));
        return time;
    }
}
