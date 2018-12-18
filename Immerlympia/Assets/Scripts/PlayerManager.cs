using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    
    public delegate void PlayerNumberDelegate(int playerNumber);
	public static event PlayerNumberDelegate characterDeathEvent;

    [HideInInspector] public PlayerControlling[] players;
    private PlayerData[] playerData;
    public static PlayerManager current;

    [Header("game meta data")]
    public float timeToRespawn = 3f;


    void Awake() {
        current = this;    
        players = new PlayerControlling[transform.childCount];
        playerData = new PlayerData[transform.childCount];
        for (int i = 0; i < players.Length; i++) {
            players[i] = transform.GetChild(i).GetComponent<PlayerControlling>();
            players[i].playerIndex = i;
            playerData[i] = transform.GetChild(i).GetComponent<PlayerData>();
            players[i].gameObject.SetActive(false);
        }
        foreach(HeroPick hp in Resources.LoadAll<HeroPick>("PickableHeroes/")){
            if(hp.currentPlayer != -1){
                playerData[hp.currentPlayer].SetupPlayerVisuals(hp);
                players[hp.currentPlayer].gameObject.SetActive(true);
            }
        }
    }

    public void CharacterDeath(int playerID){
        if(characterDeathEvent != null)
            characterDeathEvent(playerID);
    }

    public void RespawnPlayer(int playerID){

    }
}
