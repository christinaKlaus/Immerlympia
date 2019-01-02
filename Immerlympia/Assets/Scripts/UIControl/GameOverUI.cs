using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameOverUI : MonoBehaviour {

	public static GameOverUI current;
	[SerializeField] Selectable firstSelectedOnGameEnd = null;
	[SerializeField] GameObject[] activateOnGameEnd = null;
	
	void Awake(){
		current = this;
		PlayerManager.gameEndEvent += OnGameEnded;
	}

	void Start(){
		gameObject.SetActive(false);
	}

	public void OnGameEnded(){
		gameObject.SetActive(true);
		// foreach(GameObject g in activateOnGameEnd){
		// 	g.SetActive(true);
		// }
		//Debug.Log("Event ist angekommen");
		EventSystem.current.SetSelectedGameObject(firstSelectedOnGameEnd.gameObject);
		Time.timeScale = 0;
	}

	void OnDestroy(){
		PlayerManager.gameEndEvent -= OnGameEnded;
	}

}
