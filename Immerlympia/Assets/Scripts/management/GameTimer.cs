using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using System.Text.RegularExpressions;

public class GameTimer : MonoBehaviour {

    //public List<string> args;
    private bool halfTimeReached;
    GameMusicScript gameMusic;
    public static GameTimer current;
    [SerializeField] public float gameTimerDelay = 2f;
    public float playTime;
    private float currentTime;
    private PlayerManager playerManager;
    private CoinSpawnManager coinSpawnManager;
    private WaitForSeconds oneSecond;
    private Coroutine currentGameTimerRoutine;

    public float CurrentTime {
        get { return currentTime; }
    }

    // Use this for initialization
	void Awake () {
        List<string> args = new List<string>(System.Environment.GetCommandLineArgs());
        string playTimeCommand = "-playTime";
        string resultString = "noresult";
        int newPlayTime = int.MinValue;
        for(int i = 0; i < args.Count; i++){
            string s = args[i];
            // Debug.Log(s);
            if(s.Contains(playTimeCommand)){
                // Debug.Log(s + " contains " + playTimeCommand);
                resultString = Regex.Match(s, @"\d+").Value;
                // Debug.Log(resultString);
                newPlayTime = System.Int32.Parse(resultString);
                if(newPlayTime == int.MinValue)
                    continue;
                else 
                    playTime = Mathf.Abs(newPlayTime);
            }
        }

        coinSpawnManager = GetComponent<CoinSpawnManager>();

        oneSecond = new WaitForSeconds(1f);
        halfTimeReached = false;
        gameMusic = FindObjectOfType<GameMusicScript>();
        current = this;
        playerManager = FindObjectOfType<PlayerManager>();
        PlayerManager.startWinCamEvent += OnWinCamStarted;
    }

    void Start(){
        currentGameTimerRoutine = StartCoroutine(GameTimeRoutine(playTime));
    }

    IEnumerator GameTimeRoutine(float maxPlayTime){
        currentTime = maxPlayTime;
        yield return new WaitForSeconds(gameTimerDelay);

        List<string> args = new List<string>(System.Environment.GetCommandLineArgs());
        coinSpawnManager.CoinSpawnActive = args.Contains("-noCoins") ? false : true;

        // gameMusic.TransitionTo(GameMusicScript.GameMusicState.Main, gameTimerDelay);
        //Debug.Log("started game time routine", this);
        while (currentTime > maxPlayTime * 0.5f) {
            currentTime -= Time.deltaTime;
            yield return null;
        }
        //gameMusic.TransitionToClimaxFadeUp(maxPlayTime * 0.4f);
        halfTimeReached = true;

        while(currentTime > 0){
            currentTime -= Time.deltaTime;
            yield return null;
        }
        // gameMusic.TransitionTo(GameMusicScript.GameMusicState.Loop, 0.1f);
        playerManager.DetermineGameEnd();
        currentTime = 0;
    }

    void OnWinCamStarted() {
        // gameMusic.TransitionTo(GameMusicScript.GameMusicState.Menu, 1f);
        List<PlayerControlling> winner = new List<PlayerControlling>();
        //gameMusic.TransitionToGameEnd(0.0f);
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
        PlayerManager.startWinCamEvent -= OnWinCamStarted;
    }
}
