﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

	public void StartGame(){
		SceneManager.LoadScene("Immerlympia_Game");
	}
}
