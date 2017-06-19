using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	}

	public void endGame(){
		text.text = "Game Over";
		Time.timeScale = 0;
		back.gameObject.SetActive(true);
	}

}
