using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour {

    public int index;
    Text text;

    private void Start() {
        text = GetComponent<Text>();
        PlayerManager.current.players[index].updateScoreEvent.AddListener(UpdateScore);
        UpdateScore();
    }

    void UpdateScore() {
        text.text = "" + PlayerManager.current.players[index].score;
    } 
}
