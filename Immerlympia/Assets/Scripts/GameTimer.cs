using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class GameTimer : MonoBehaviour {

    private bool halfTimeReached;
    GameMusicScript gameMusic;
   
    public static GameTimer current;
    public UnityEvent gameEndEvent;
    public float playTime;
    private float currTime;

    // Use this for initialization
	void Awake () {
        halfTimeReached = false;
        gameMusic = GetComponent<GameMusicScript>();
        currTime = 0;
        current = this;
    }
	
	// Update is called once per frame
	void Update () {
        if (currTime < playTime) {
            currTime += Time.deltaTime;

            if(!halfTimeReached && currTime > playTime / 4)
            {
                gameMusic.TransitionToClimaxFadeUp(playTime / 3);
                halfTimeReached = true;
            }

            if (currTime >= playTime) {
                GameEnd();
            }
        }
    }

    void GameEnd() {
        List<PlayerControlling> winner = new List<PlayerControlling>();
        gameEndEvent.Invoke();
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
            Debug.Log("Player " + (p.playerNumber+1) + "\tScore: " + p.score);


        
    }

    public float getCurrentTime()
    {
        return currTime;
    }
}
