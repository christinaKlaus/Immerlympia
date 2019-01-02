﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour {
    
    public delegate void PlayerNumberDelegate(int playerNumber);
	public static event PlayerNumberDelegate characterDeathEvent;

    public delegate void GameStateDelegate();
    public static event GameStateDelegate gameEndEvent;

    public bool RespawnActive {
        get { return respawnActive; }
    }
    [HideInInspector] public PlayerControlling[] players;
    public static PlayerManager current;

    [SerializeField] private bool respawnActive = true;

    [Header("game meta data")]
    public float timeToRespawn = 3f;
    [Header("winner cam")]
    public float winCamDuration = 5f;
    public float  winCamSpeed = 1f;

    
    private PlayerData[] playerData;
    private Coroutine[] respawnRoutines;
    [SerializeField] CinemachineVirtualCamera vcamWinner = null;

    void Awake() {
        current = this;    
        int playerAmount = transform.childCount;
        players = new PlayerControlling[playerAmount];
        playerData = new PlayerData[playerAmount];
        respawnRoutines = new Coroutine[playerAmount];
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
        players[playerID].gameObject.SetActive(false);
        if(characterDeathEvent != null)
            characterDeathEvent(playerID);

        if(respawnActive)
            respawnRoutines[playerID] = StartCoroutine(RespawnPlayer(playerID));
        else if(ActivePlayers() == 1){
            // fire game end event
            if(gameEndEvent != null)
                gameEndEvent();
            // setup winner cam
            StartCoroutine(ActivateWinCam(WinningPlayer()));
        }
    }

    public IEnumerator RespawnPlayer(int playerID){
        yield return new WaitForSeconds(timeToRespawn);
        if(respawnActive)
            players[playerID].gameObject.SetActive(true);
        else {
            CharacterDeath(playerID);
        }
    }

    IEnumerator ActivateWinCam(PlayerControlling winner){
        CinemachineSmoothPath camPath = winner.GetComponent<PlayerData>().winCamTrack;
        CinemachineTrackedDolly trackedDolly = vcamWinner.GetCinemachineComponent<CinemachineTrackedDolly>();
        vcamWinner.LookAt = winner.transform;
        vcamWinner.Follow = winner.transform;
        trackedDolly.m_Path = camPath;
        vcamWinner.Priority = 10;
        trackedDolly.m_PathPosition = 0f;
        Debug.Log("Winner: " + winner.name + " with score: " + winner.score, winner);
        Debug.Log("Winner cam path of: " + camPath.transform.parent.name, camPath);
        float position = 0f;
        yield return new WaitForEndOfFrame();
        for(float t = 0f; t < winCamDuration; t = t + Time.fixedUnscaledDeltaTime){
            position += winCamSpeed * Time.fixedUnscaledDeltaTime;
            //trackedDolly.m_PathPosition += winCamSpeed * Time.fixedUnscaledDeltaTime;
            trackedDolly.m_PathPosition = position % 1f;
            yield return null;
        }
        yield return null;
    }

    int ActivePlayers(){
        int activePlayers = 0;
        foreach(PlayerControlling pc in players){
            if(pc.gameObject.activeSelf) activePlayers++;
        }
        return activePlayers;
    }

    /*
        1. game timer end
        2. do any of the active players have equal score?
            a. yes: keep game running until last-man-standing -> 2.b.
            b. no: put solo high score player front and center
        3. game end to menu
    */

    PlayerControlling WinningPlayer(){
        int highScore = 0;
        PlayerControlling winner = null;
        foreach(PlayerControlling pc in players){
            if(pc.playerIndex == -1) continue;
            Debug.Log("Determining: " + pc.name);
            if(winner == null){
                Debug.Log("winner was null, is now " + pc.name);
                winner = pc;
                highScore = winner.score;
            } else if(pc.score > winner.score){
                Debug.Log(pc.name + "(" + pc.score + ") has higher score than " + winner + "(" + winner.score + ") [highScore: " + highScore + "]");
                winner = pc;
                highScore = winner.score;
            } else if(pc.score == winner.score){
                Debug.Log(pc.name + " has same score (" + pc.score + ") as " + winner.name + "(" + winner.score + "). Determining death time. [highScore: " + highScore + "]");
                Debug.Log(pc.name + ": " + pc.timeOfLastDeath + " | " + winner.name + ": " + winner.timeOfLastDeath + " -> RESULT: " + (pc.timeOfLastDeath > winner.timeOfLastDeath ? pc.name : winner.name));
                winner = pc.timeOfLastDeath > winner.timeOfLastDeath ? pc : winner;
                highScore = winner.score;
            }
        }
        return winner;
    }

    /// <summary>Toggle respawn on (true), off (false) or opposite state (null)
    public void ToggleRespawn(bool? onOff){
        if(onOff == null)
            respawnActive = !respawnActive;
        else{
            bool b = (bool) onOff;
            respawnActive = b;
            if(!b){
                for(int i = 0; i < respawnRoutines.Length; i++){
                    if(respawnRoutines[i] != null){
                        StopCoroutine(respawnRoutines[i]);
                        CharacterDeath(players[i].playerIndex);
                    }
                }
            }
        }
    }

}

public class ScoreComparer : IComparer<PlayerControlling>
{
    public int Compare(PlayerControlling x, PlayerControlling y)
    {
        // sign:
        // + if x > y
        // 0 if x == y
        // - if x < y
        return x.score - y.score;
    }
}