using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameMusicScript : MonoBehaviour {

    public enum GameMusicState{
        Start,
        Main,
        Loop,
        Menu
    }

    [SerializeField] AudioMixerGroup musicMainGroup;
    [SerializeField] AudioMixerSnapshot gameStart, gameRunning, gameRunningLoop, gameMenuLoop;
    [SerializeField] AudioSource gameTrackFullS, gameTrackLoopS, gameMenuLoopS;

    void Awake(){
        GameTimer gameTimer = FindObjectOfType<GameTimer>();

        List<string> args = new List<string>(System.Environment.GetCommandLineArgs());
        if(args.Contains("-noDancing")) return;
        else {
            TransitionTo(GameMusicState.Start, 0f);
            double allSourcesStartTime = AudioSettings.dspTime + gameTimer.gameTimerDelay;
            gameTrackFullS.PlayScheduled(allSourcesStartTime);
            gameTrackLoopS.PlayScheduled(allSourcesStartTime);
            gameMenuLoopS.PlayScheduled(allSourcesStartTime);
        }
    }

    public void TransitionTo(GameMusicState state, float transitionTime){
        switch(state){
            case GameMusicState.Start:
                gameStart.TransitionTo(transitionTime);
                break;
            case GameMusicState.Main:
                gameRunning.TransitionTo(transitionTime);
                break;
            case GameMusicState.Loop:
                gameRunningLoop.TransitionTo(transitionTime);
                break;
            case GameMusicState.Menu:
                gameMenuLoop.TransitionTo(transitionTime);
                break;
        }
    }


    // void Start () {
    //     pMan = GameObject.FindObjectOfType<PlayerManager>();

    //     //building the soundsources and setting them up
    //     introS = gameObject.AddComponent<AudioSource>();
    //     loopS = gameObject.AddComponent<AudioSource>();
    //     climaxS = gameObject.AddComponent<AudioSource>();
    //     introS.outputAudioMixerGroup = musicMixer.audioMixer.FindMatchingGroups("main")[0];
    //     loopS.outputAudioMixerGroup = musicMixer.audioMixer.FindMatchingGroups("main")[0];
    //     climaxS.outputAudioMixerGroup = musicMixer.audioMixer.FindMatchingGroups("climax")[0];

    //     audioInitTime = AudioSettings.dspTime;
    //     introS.clip = intro;
    //     introS.PlayScheduled(audioInitTime + audioStartDelay);
    //     climaxS.clip = climax;
    //     climaxS.PlayScheduled(audioInitTime + audioStartDelay);

    //     loopS.clip = loop;
    //     loopS.loop = true;

    //     loopS.PlayScheduled(audioInitTime + audioStartDelay + intro.length);
    //     Destroy(introS, intro.length);

    //     //Invoke("EvaluatePlayerScore", (float) (audioInitTime + audioStartDelay + 36.0f));
    // }

    /*void EvaluatePlayerScore()
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

        AudioSource winS = gameObject.AddComponent<AudioSource>();
        winS.outputAudioMixerGroup = loopS.outputAudioMixerGroup = musicMixer.audioMixer.FindMatchingGroups("winClip")[0];

        //winS.clip = playerWinTracks[winningPlayerID];
        winS.PlayScheduled(audioInitTime + audioStartDelay + 38.4f);
    }*/

    // public void TransitionToClimaxFadeUp(float transitionTime)
    // {
    //     climaxGroupFadeUp.TransitionTo(transitionTime);
    // }

    // public void TransitionToGameEnd(float transitionTime)
    // {
    //     gameEnd.TransitionTo(transitionTime);
    // }

}
