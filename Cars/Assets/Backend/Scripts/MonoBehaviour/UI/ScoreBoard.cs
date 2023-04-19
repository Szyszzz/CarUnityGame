using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI fullscore;
    public TextMeshProUGUI score;
    public TextMeshProUGUI multiplier;
    public TextMeshProUGUI weight;

    public void UpdateNumbers(TextMeshProUGUI tochange, string text)
    {
        tochange.text = text;
    }
}
