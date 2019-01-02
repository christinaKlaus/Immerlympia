﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICameraPosition : MonoBehaviour, ISelectHandler {

	[Range(0,2), Tooltip("0 = play, 1 = settings, 2 = quit")]
	public int menuPosition;
	private int goToMenuHash = -1;

	private MainMenuInput mainMenuInput;
	
	public float WorldAngle {
		get {
			return Vector3.SignedAngle(-Vector3.forward, new Vector3(transform.position.x, 0f, transform.position.z), Vector3.up);
		}
	}

	void Start(){
		mainMenuInput = GetComponentInParent<MainMenuInput>();
		goToMenuHash = mainMenuInput.goToMenuHash;
	}

    public void OnSelect(BaseEventData eventData)
    {
		mainMenuInput.RotateMenu(WorldAngle);
    }

}
