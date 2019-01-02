using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInformation : MonoBehaviour {

	private UIPlayerRespawnTimer[] respawnTimers;
	private ScoreUpdate[] scoreUpdaters;
	private PlayerManager playerManager;
	private Mask[] playerMasks;

	public void Awake(){
		playerManager = FindObjectOfType<PlayerManager>();
		respawnTimers = new UIPlayerRespawnTimer[4];
		scoreUpdaters = new ScoreUpdate[4];
		playerMasks = new Mask[4];
		foreach(Transform t in transform){
			t.gameObject.SetActive(false);
		}
		foreach(HeroPick hp in Resources.LoadAll<HeroPick>("PickableHeroes/")){
            if(hp.currentPlayer != -1){
				int playerID = hp.currentPlayer;
                Transform parent = transform.GetChild(playerID);
				parent.gameObject.SetActive(true);
				
				respawnTimers[playerID] = parent.GetComponentInChildren<UIPlayerRespawnTimer>();
				respawnTimers[playerID].playerIndex = playerID;

				playerMasks[playerID] = parent.GetComponentInChildren<Mask>();
				playerMasks[playerID].showMaskGraphic = false;
				
				scoreUpdaters[playerID] = parent.GetComponentInChildren<ScoreUpdate>();
				scoreUpdaters[playerID].playerIndex = playerID;
				scoreUpdaters[playerID].SetTextColor(hp.heroColor);
            }
        }
		PlayerManager.characterDeathEvent += OnPlayerDeath;
	}

	private void OnPlayerDeath(int playerID){
		if(respawnTimers[playerID] != null){
			if(playerManager.RespawnActive){
				Debug.Log("Respawn timer started for player " + playerID);
				respawnTimers[playerID].StartTimer(playerManager.timeToRespawn);
			} else {
				respawnTimers[playerID].MarkPlayerInactive();
				playerMasks[playerID].showMaskGraphic = true;
			}
		}
	}

	void OnDestroy(){
		PlayerManager.characterDeathEvent -= OnPlayerDeath;
	}

}
