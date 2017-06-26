using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour {

    private int index;
    Text text;

    private void Start() {

        index = transform.GetSiblingIndex();
        text = GetComponent<Text>();
        PlayerManager.current.players[index].increaseScoreEvent.AddListener(UpdateScore);
        UpdateScore();
    }

    void UpdateScore() {
        text.text = "Player " + (index + 1) + ": " + PlayerManager.current.players[index].score;
    } 
}
