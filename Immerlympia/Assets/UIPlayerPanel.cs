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
	[SerializeField] private Image activeHeroImage;
	[SerializeField] private TMPro.TextMeshProUGUI characterText, playerText;
	[SerializeField] private ParticleSystem heroLockedPS;

	public void Awake(){
		if(activeHeroImage == null)
			throw new Exception("ActiveHeroImage on " + gameObject + " not set!");
		
		if(playerName.Length == 0){
			playerName = "Player" + playerNumber;
		}
		playerText.SetText(playerName);
	}

	public void SetHeroPick(HeroPick newHero){
		if(heroLocked) return;

		if(!joinedBefore) joinedBefore = true;

		newHero.isPicked = true;
		newHero.currentPlayer = playerNumber;
		
		currentPick = newHero;
		activeHeroImage.sprite = newHero.heroSprite;
		characterText.SetText(newHero.heroName);
		characterText.color = newHero.heroNameColor;
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
		if(lockHero){
			ParticleSystem.MainModule mainModule = heroLockedPS.main;
			mainModule.startColor = currentPick.heroNameColor;
			heroLockedPS.Play();
		}
		else {
			heroLockedPS.Stop();
			heroLockedPS.Clear();
		}
	}

	public bool HasJoinedBefore(){
		return joinedBefore;
	}
}
