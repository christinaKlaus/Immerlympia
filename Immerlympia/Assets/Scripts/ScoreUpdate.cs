using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreUpdate : MonoBehaviour {

    [ReadOnly(false)] public int playerIndex = -1;
    public string intFormat = "00";
    TextMeshProUGUI text;

    private void Awake() {
        text = GetComponent<TextMeshProUGUI>();
        text.SetText("0");
        PlayerControlling.UpdateScoreEvent += UpdateScore;
    }

    public void SetTextColor(Color color){
        text.color = color;
    }

    public void UpdateScore(int playerIndex, int score){
        if(playerIndex == this.playerIndex){
            text.SetText(score.ToString(intFormat));
        }
    }
}
