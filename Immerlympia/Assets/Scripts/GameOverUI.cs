using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameOverUI : MonoBehaviour {

	public static GameOverUI current;
	
	Text text;
	Button back;

	void Awake(){
		current = this;
		back = GetComponent<Button>();
		back.gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		GameTimer.current.gameEndEvent.AddListener(endGame);
	}

	public void endGame(){
		Debug.Log("Event ist angekommen");
		text.text = "Game Over";
		back.gameObject.SetActive(true);
		Time.timeScale = 0;
	}

}
