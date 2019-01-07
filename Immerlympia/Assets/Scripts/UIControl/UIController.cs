﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

	public void StartGame(){
		SceneManager.LoadScene("Immerlympia_Game");
		Time.timeScale = 1;
	}

	public void BackToMenu(){
		SceneManager.LoadScene("main_menu_new");
	}
	
	public void ExitGame(){
		Application.Quit();
	}

	public void Credits(){
		SceneManager.LoadScene("Credits");
	}
}
