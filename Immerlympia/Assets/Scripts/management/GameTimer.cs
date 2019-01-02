﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class GameTimer : MonoBehaviour {

    private bool halfTimeReached;
    GameMusicScript gameMusic;
    public static GameTimer current;
    public float playTime;
    private float currentTime;
    private PlayerManager playerManager;
    private WaitForSeconds oneSecond;
    private Coroutine currentGameTimerRoutine;

    public float CurrentTime {
        get { return currentTime; }
    }

    // Use this for initialization
	void Awake () {
        oneSecond = new WaitForSeconds(1f);
        halfTimeReached = false;
        gameMusic = GetComponent<GameMusicScript>();
        current = this;
        playerManager = FindObjectOfType<PlayerManager>();
        PlayerManager.gameEndEvent += GameEnd;
    }

    void Start(){
        
        currentGameTimerRoutine = StartCoroutine(GameTimeRoutine(playTime));
    }

    IEnumerator GameTimeRoutine(float maxPlayTime){
        currentTime = maxPlayTime;
        Debug.Log("started game time routine", this);
        while (currentTime > 0) {
            currentTime -= Time.deltaTime;

            if(!halfTimeReached && currentTime < maxPlayTime * 0.5f)
            {
                gameMusic.TransitionToClimaxFadeUp(maxPlayTime * 0.4f);
                halfTimeReached = true;
            }

            if (currentTime <= 0) {
                playerManager.ToggleRespawn(false);
            }
            yield return null;
        }
        currentTime = 0;
    }

    void GameEnd() {
        List<PlayerControlling> winner = new List<PlayerControlling>();
        gameMusic.TransitionToGameEnd(0.0f);
        foreach (PlayerControlling p in PlayerManager.current.players) {
            if (winner.Count == 0 || p.score > winner[0].score) {
                winner.Clear();
                winner.Add(p);
            } else if(p.score == winner[0].score) {
                winner.Add(p);
            }
        }

        foreach (PlayerControlling p in winner)
            Debug.Log("Player " + (p.playerIndex+1) + "\tScore: " + p.score);
        
    }

    public void OnDestroy(){
        PlayerManager.gameEndEvent -= GameEnd;
    }
}