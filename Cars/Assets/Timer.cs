using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerUI;

    void Update()
    {
        timerUI.text = formatTime(TimeManager.instance.RemainingTime);
    }

    private string formatTime(Vector2 time)
    {
        string output;
        if (time.x < 10 && time.x > 0)
            output = "0" + time.x + ":";
        else if (time.x <= 0)
            output = "00:";
        else
            output = time.x + ":";

        if (time.y < 10 && time.y > 0)
            output += "0" + time.y;
        else if (time.y <= 0)
            output += "00";
        else
            output += time.y;

        return output;
    }
}
