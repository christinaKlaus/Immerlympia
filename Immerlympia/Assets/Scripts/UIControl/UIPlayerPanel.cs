using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerPanel : MonoBehaviour {

	public int playerNumber = -1;	
	public string playerName;
	[ReadOnly(false)] public HeroPick currentPick;
	[SerializeField, ReadOnly(false)] private bool joinedBefore = false, heroLocked = false;
	[SerializeField] private Image activeHeroImage = null;
	[SerializeField] public TMPro.TextMeshProUGUI characterText = null, playerText = null;
	[SerializeField] private ParticleSystem heroLockedPS = null;
	[SerializeField] private Image heroLockedBorder = null;
	ParticleSystem.MainModule heroLockedPSMainModule;

	public void OnEnable(){
		if(activeHeroImage == null)
			throw new Exception("ActiveHeroImage on " + gameObject + " not set!");
		
		// Debug.Log("Enabling Player Slot " + playerNumber);
		// if(playerName.Length == 0){
		// 	playerName = "Player" + playerNumber;
		// }
		// playerText.SetText(playerName);
		if(!heroLockedPS){
			heroLockedPS = GetComponentInChildren<ParticleSystem>();
		}
		heroLockedPSMainModule = heroLockedPS.main;
	}

	public void SetHeroPick(HeroPick newHero){
		if(heroLocked) return;

		if(!joinedBefore) joinedBefore = true;

		newHero.isPicked = true;
		newHero.currentPlayer = playerNumber;
		
		currentPick = newHero;
		activeHeroImage.sprite = newHero.heroSprite;
		characterText.SetText(newHero.heroName);
		characterText.color = newHero.heroColor;
	}

	public void UnsetHeroPick(){
		if(currentPick == null || heroLocked) return;
		
		characterText.SetText("-");
		characterText.color = Color.white;
		//Debug.Log("Unsetting hero " + currentPick.name + ": " + currentPick.heroName);
		currentPick.currentPlayer = -1;
		currentPick.isPicked = false;
		//Debug.Log("Unset hero " + currentPick.name + ": " + currentPick.heroName);
	}

	public void LockUnlockHeroPick(bool lockHero){
		heroLocked = lockHero;
		heroLockedBorder.enabled = lockHero;
		if(lockHero){
			// ParticleSystem.MainModule mainModule = heroLockedPS.main;
			// mainModule.startColor = currentPick.heroColor;
			heroLockedPSMainModule.startColor = currentPick.heroColor;
			heroLockedPS.Play();
		}
		else {
			heroLockedPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
	}

	public bool HasJoinedBefore(){
		return joinedBefore;
	}
}
