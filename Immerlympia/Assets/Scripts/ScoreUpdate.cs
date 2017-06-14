using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour {

    Text textComponent;
    GameObject canvas;
    Text[] children;


    private void Start() {
        canvas = GameObject.FindGameObjectWithTag("UI");
        children = canvas.GetComponentsInChildren<Text>();
        for(int i = 0; i < children.Length; i++) {
            Debug.Log(children[i]);
            textComponent = children[i].GetComponent<Text>();
            textComponent.text = "Player " + i + ": " + 0;
        }
        
    }

    public void updateScore(int score, int playerNumber){
        textComponent.text = "Player " + playerNumber + ": " + score;
    }
}
