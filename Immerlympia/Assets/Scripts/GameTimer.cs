using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour {

    public static GameTimer current;
    public UnityEvent gameEndEvent;
    public float playTime;
    private float currTime;

    // Use this for initialization
	void Awake () {
        currTime = 0;
        current = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (currTime < playTime) {
            currTime += Time.deltaTime;

            if (currTime >= playTime) {
                GameEnd();
            }
        }
    }

    void GameEnd() {
        List<PlayerController> winner = new List<PlayerController>();
        gameEndEvent.Invoke();
        
        foreach (PlayerController p in PlayerManager.current.players) {
            if (winner.Count == 0 || p.score > winner[0].score) {
                winner.Clear();
                winner.Add(p);
            } else if(p.score == winner[0].score) {
                winner.Add(p);
            }
        }

        foreach (PlayerController p in winner)
            Debug.Log("Player " + (p.playerNumber+1) + "\tScore: " + p.score);


        
    }

    public float getCurrentTime()
    {
        return currTime;
    }
}
