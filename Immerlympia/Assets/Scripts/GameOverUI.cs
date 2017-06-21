using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameOverUI : MonoBehaviour {

	public static GameOverUI current;
	
	Graphic[] gameOverGraphics;
	Selectable[] selectables;
	void Awake(){
		current = this;
		gameOverGraphics = GetComponentsInChildren<Graphic>();
		foreach (Graphic g in gameOverGraphics)
			g.enabled = false;
		
		selectables = GetComponentsInChildren<Selectable>();
		foreach (Selectable s in selectables)
			s.enabled = false;
		
	}

	// Use this for initialization
	void Start () {
		GameTimer.current.gameEndEvent.AddListener(endGame);
	}

	public void endGame(){
		foreach (Graphic g in gameOverGraphics)
			g.enabled = true;
		foreach (Selectable s in selectables)
			s.enabled = true;
		Debug.Log("Event ist angekommen");
		Time.timeScale = 0;
	}

}
