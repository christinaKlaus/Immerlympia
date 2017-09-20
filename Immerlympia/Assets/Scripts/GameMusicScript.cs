using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameMusicScript : MonoBehaviour {

    public AudioClip intro;
    public AudioClip loop;
    public AudioClip highScoreStingerTest;
    public AudioMixerGroup musicMixer;
    public AudioMixerSnapshot[] playerWinSnaps;

    AudioSource loopS;
    AudioSource introS;
    GameTimer gTimer;
    public PlayerManager pMan;

    float gTime;



    void Start () {
        introS = gameObject.AddComponent<AudioSource>();
        loopS = gameObject.AddComponent<AudioSource>();
        introS.outputAudioMixerGroup = musicMixer.audioMixer.FindMatchingGroups("main")[0];
        loopS.outputAudioMixerGroup = musicMixer.audioMixer.FindMatchingGroups("main")[0];

        introS.clip = intro;
        introS.Play();

        
        loopS.clip = loop;
        loopS.loop = true;

        loopS.PlayDelayed(intro.length);
        Destroy(introS, intro.length);

        gTimer = GetComponent<GameTimer>();
        gTime = gTimer.playTime;


        Invoke("EvaluatePlayerScore", gTime - 20);

        pMan = GameObject.FindObjectOfType<PlayerManager>();

        
    }


    void EvaluatePlayerScore()
    {
        if (pMan == null)
            Debug.Log("GameMusicScript hasn't found PlayerManager");

        int[] playerScores = new int[pMan.players.Length];

        for(int i = 0; i < pMan.players.Length; i++)
        {
            playerScores[i] = pMan.players[i].score;
            for(int e = 0; e < i; e++)
            {
                if(playerScores[e] < playerScores[i])
                {
                    playerScores[e] = -1;
                } else
                {
                    playerScores[i] = -1;
                }
            }

        }

        int winningPlayerID = -1;
        while(winningPlayerID == -1)
        {
            int randomArrayPlace = (int)Random.Range(0, playerScores.Length);
            winningPlayerID = (playerScores[randomArrayPlace] != -1) ? randomArrayPlace : -1;
        }

        Debug.Log("Winning player is No. " + winningPlayerID);

        playerWinSnaps[winningPlayerID].TransitionTo(1f);



    }

}
