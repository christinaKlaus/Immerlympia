using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIPlayerJoin : MonoBehaviour {

	[SerializeField] private bool resetHeropicksOnStart = true;

	[SerializeField] private float inputDelay = 1.5f;
	private bool readyActive = false;
	[SerializeField, ReadOnly(false)] bool anyoneJoined = false, allLockedIn = false;
	private bool[] zeroed, joined;
	private float[] lastInputTimes;
	private string[] leaveButtonNames = new string[]{ "Cancel0", "Cancel1", "Cancel2", "Cancel3"};
	private string[] joinButtonNames = new string[]{ "Jump0", "Jump1", "Jump2", "Jump3"};
	private string[] horizontalAxisNames = new string[]{ "Horizontal0", "Horizontal1", "Horizontal2", "Horizontal3"};
	[SerializeField] private UIPlayerPanel[] playerPanels = null;
	[SerializeField] private HeroPick[] pickableHeroes = null;
	[Space, SerializeField] private Selectable selectOnNoJoinedCancel = null, selectOnAllLockedIn = null;
	[SerializeField] UnityEvent OnNoJoinedCancel = null;
	private PlayerJoinState[] joinStates;

	void Start(){
		foreach(UIPlayerPanel p in playerPanels){
			if(p.gameObject.activeSelf)
				p.gameObject.SetActive(false);
		}

		joinStates = new PlayerJoinState[playerPanels.Length];
		zeroed = new bool[playerPanels.Length];
		joined = new bool[playerPanels.Length];
		lastInputTimes = new float[playerPanels.Length];

		for(int i = 0; i < playerPanels.Length; i++){
			joinStates[i] = PlayerJoinState.Open;
			playerPanels[i].playerNumber = i;
		}
		
		selectOnAllLockedIn.gameObject.SetActive(false);

		if(resetHeropicksOnStart){
			foreach(HeroPick hp in pickableHeroes){
				hp.currentPlayer = -1;
				hp.isPicked = false;
			}
			gameObject.SetActive(false);
		} else {
			SetCurrentPicks();
		}
		
	}

	void SetCurrentPicks(){
		Debug.Log("Setting current picks again");
		foreach(UIPlayerPanel uipp in playerPanels){
			uipp.LockUnlockHeroPick(false);
			uipp.UnsetHeroPick();
		}
		foreach(HeroPick hp in pickableHeroes){
			if(hp.currentPlayer != -1){
				Debug.Log(hp.name + " picked by player " + hp.currentPlayer + " last round");
				int currentPlayer = hp.currentPlayer;
				playerPanels[currentPlayer].gameObject.SetActive(true);
				joinStates[currentPlayer] = PlayerJoinState.Joined;
				joined[currentPlayer] = true;
				playerPanels[currentPlayer].SetHeroPick(hp);
			}
		}
	}

	public void Update(){
		anyoneJoined = AnyoneJoined();
		allLockedIn = AllJoinedLockedIn(); 

		if(Input.GetButtonDown(leaveButtonNames[0]) && !anyoneJoined){
			//Debug.Log("No one joined, canceling");
			EventSystem.current.SetSelectedGameObject(selectOnNoJoinedCancel.gameObject);
			gameObject.SetActive(false);
			if(OnNoJoinedCancel != null)
				OnNoJoinedCancel.Invoke();
			return;
		}

		for(int i = 0; i < playerPanels.Length; i++){
			bool submitButtonDown = Input.GetButtonDown(joinButtonNames[i]);
			bool cancelButtonDown = Input.GetButtonDown(leaveButtonNames[i]);
			
			float horizontalAxis = Input.GetAxis(horizontalAxisNames[i]);

			// if(Mathf.Abs(horizontalAxis) > 0.4f){
			// 	Debug.Log("Axis " + i + ": " + horizontalAxis);
			// }

			switch(joinStates[i]){
				case PlayerJoinState.Open:
					if(submitButtonDown){
						playerPanels[i].gameObject.SetActive(true);
						joinStates[i] = PlayerJoinState.Joined;
						joined[i] = true;
						if(!playerPanels[i].HasJoinedBefore()){
							//Debug.Log("Player " + i + " has not joined before");
							foreach(HeroPick hp in pickableHeroes){
								if(!hp.isPicked){
									//Debug.Log("Player " + i + "'s first hero is " + hp.name + ": " + hp.heroName);
									playerPanels[i].SetHeroPick(hp);
									break;
								}
							}
						} else if ((playerPanels[i].currentPick.isPicked && playerPanels[i].currentPick.currentPlayer != playerPanels[i].playerNumber)){
							//Debug.Log("Player " + i + " joined before with " + playerPanels[i].currentPick.name + ": " + playerPanels[i].currentPick.heroName + " and it's unavailable");
							foreach(HeroPick hp in pickableHeroes){
								if(!hp.isPicked && hp.currentPlayer == -1){
									//Debug.Log("Player " + i + "'s new hero is " + hp.name + ": " + hp.heroName);
									playerPanels[i].SetHeroPick(hp);
									break;
								} else {
									//Debug.Log(hp.name + ": " + hp.heroName + " was not available for player " + i);
								}
							}
						} else {
							//Debug.Log("Player " + i + " joined before with " + playerPanels[i].currentPick.name + ": " + playerPanels[i].currentPick.heroName + " and it's available");
							playerPanels[i].SetHeroPick(playerPanels[i].currentPick);
						}
					}
					break;
				case PlayerJoinState.Joined:
					if(cancelButtonDown){
						playerPanels[i].UnsetHeroPick();
						joinStates[i] = PlayerJoinState.Open;
						playerPanels[i].gameObject.SetActive(false);
						joined[i] = false;
					}
					if(Mathf.Abs(horizontalAxis) > 0.5f){
						// Debug.Log("axis " + i + " cycling through heroes");
						if(Time.time - lastInputTimes[i] > inputDelay || zeroed[i]){
							int reps = 0;
							int increment = Mathf.Sign(horizontalAxis) < 0 ? pickableHeroes.Length - 1 : 1;
							int index = (Array.IndexOf<HeroPick>(pickableHeroes, playerPanels[i].currentPick) + increment) % pickableHeroes.Length;
							for(int j = index; j < pickableHeroes.Length; j = (j + increment) % pickableHeroes.Length){
								if(!pickableHeroes[j].isPicked){
									//Debug.Log("picked from cycle: " + j);
									playerPanels[i].UnsetHeroPick();
									playerPanels[i].SetHeroPick(pickableHeroes[j]);
									break;
								}
								reps++;
								if(reps > pickableHeroes.Length + 1){
									//Debug.Log("Too many players and too few heroes");
									break;
								}
							}
							lastInputTimes[i] = Time.time;
						}
						zeroed[i] = false;
					}
					if(!zeroed[i] && Mathf.Abs(horizontalAxis) < 0.001f){
						zeroed[i] = true;
					}
					if(submitButtonDown){
						joinStates[i] = PlayerJoinState.Locked;
						playerPanels[i].LockUnlockHeroPick(true);
					}
					break;
				case PlayerJoinState.Locked:
					if(cancelButtonDown){
						playerPanels[i].LockUnlockHeroPick(false);
						joinStates[i] = PlayerJoinState.Joined;
					}
					break;
			}
		}
		if(allLockedIn && !readyActive){
			selectOnAllLockedIn.gameObject.SetActive(true);
			EventSystem.current.SetSelectedGameObject(selectOnAllLockedIn.gameObject);
			readyActive = true;
		} else if (readyActive && !allLockedIn) {
			//Debug.Log("Whoops! Someone wants to rethink his choice!");
			EventSystem.current.SetSelectedGameObject(null);
			selectOnAllLockedIn.gameObject.SetActive(false);
			readyActive = false;
		}
	}

	bool AnyoneJoined(){
		foreach(bool b in joined){
			if(b) return true;
		}
		return false;
	}

	bool AllJoinedLockedIn(){
		if(!anyoneJoined) return false;

		int numJoined = 0;
		for(int i = 0; i < playerPanels.Length; i++){
			if(joined[i]){
				numJoined++;
				if(joinStates[i] != PlayerJoinState.Locked){
					return false;
				}
			}
		}
		return numJoined > 0 ? true : false;
	}


}