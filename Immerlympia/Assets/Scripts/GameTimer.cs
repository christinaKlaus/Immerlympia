using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour {

    public float playTime;
    private float currTime;

    // Use this for initialization
	void Start () {
        currTime = 0;
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

        Time.timeScale = 0;
    }
}
