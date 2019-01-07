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
		PlayerManager.activateGameOverUIEvent += OnActivateGameOverUI;
	}

	void Start(){
		gameObject.SetActive(false);
	}

	void OnActivateGameOverUI(){
		gameObject.SetActive(true);
		EventSystem.current.SetSelectedGameObject(firstSelectedOnGameEnd.gameObject);
		Time.timeScale = 0;
	}

	void OnDestroy(){
		PlayerManager.activateGameOverUIEvent -= OnActivateGameOverUI;
	}

}
