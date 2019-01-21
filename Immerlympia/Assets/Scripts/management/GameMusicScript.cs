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

    [SerializeField] ScheduledAudioEvent gameMusicSchedule = null;

    void Awake(){
        GameTimer gameTimer = FindObjectOfType<GameTimer>();

        List<string> args = new List<string>(System.Environment.GetCommandLineArgs());
        if(args.Contains("-noDancing")) return;
        else {
            gameMusicSchedule.startTime = gameTimer.gameTimerDelay;
            gameMusicSchedule.ScheduleAudio(new GameObject("gameMusicSchedule"));
        }
    }


}
