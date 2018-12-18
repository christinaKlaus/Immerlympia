using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInformation : MonoBehaviour {

	private UIPlayerRespawnTimer[] respawnTimers;
	private ScoreUpdate[] scoreUpdaters;
	private PlayerManager playerManager;

	public void Awake(){
		playerManager = GameObject.FindObjectOfType<PlayerManager>();
		respawnTimers = new UIPlayerRespawnTimer[4];
		scoreUpdaters = new ScoreUpdate[4];
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
				
				scoreUpdaters[playerID] = parent.GetComponentInChildren<ScoreUpdate>();
				scoreUpdaters[playerID].playerIndex = playerID;
				scoreUpdaters[playerID].SetTextColor(hp.heroColor);
            }
        }
		PlayerManager.characterDeathEvent += StartRespawnTimer;
	}

	private void StartRespawnTimer(int playerID){
		if(respawnTimers[playerID] == null) return;
		respawnTimers[playerID].StartTimer(playerManager.timeToRespawn);
	}

	void OnDestroy(){
		PlayerManager.characterDeathEvent -= StartRespawnTimer;
	}

}
