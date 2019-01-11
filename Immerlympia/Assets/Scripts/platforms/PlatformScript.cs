using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement.TweenSystem;
using Pixelplacement;

public class PlatformScript : MonoBehaviour {

    public AudioClip[] descendClips;
    public Vector2 lifetimeRange = new Vector2(10, 20);
    public float fallSpeed = 11f;
    public float warning = 5f;
    public Vector3 shakiness = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 descendTilt = new Vector3(0f, 0f, -45f);
    public float descendTiltTime = 2.5f;
    public float appearTime = 3f;

    public bool isMoving = true;
    
    [HideInInspector] public bool canFall = true;
    [HideInInspector] public int currentPosition = -1;

    
    private float timer;
    private float duration;
    private bool playedDescendSound = false;
    private bool coinSpawnsActive = false;
    private bool gameEnded = false;

    private Vector3[] basePos;
    private AudioSource platformAudio;
    private CoinSpawnPoint[] spawns;
    private platformDirtParticleSystem[] platformEdgeDirt;
    private PlatformSpawn platformSpawn;
    private Coroutine shakeRoutine;
    private TweenBase rotateTween, shakeTween;

    void Awake(){
        gameEnded = false;
        PlayerManager.startWinCamEvent += OnWinCamActivated;
    }

    void OnEnable () {
        timer = Random.Range(lifetimeRange.x, lifetimeRange.y);
        duration = timer;

        if(platformSpawn == null)
            platformSpawn = GetComponentInParent<PlatformSpawn>();

        if(spawns == null)
            spawns = GetComponentsInChildren<CoinSpawnPoint>();

        if(platformEdgeDirt == null)
            platformEdgeDirt = GetComponentsInChildren<platformDirtParticleSystem>();
        
        if(descendClips.Length == 0) {
            Debug.Log("Platform prefab " + gameObject.name + " is missing descend sounds");
        }
        
        if(platformAudio == null) {
            platformAudio = GetComponent<AudioSource>();
        }

        if (basePos == null) {
            basePos = new Vector3[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
                basePos[i] = transform.GetChild(i).localPosition;
        }

        platformAudio.volume = 1;
        Update();
    }

    void Update () {
        if(!isMoving && !coinSpawnsActive){
            foreach(CoinSpawnPoint s in spawns){
                // Debug.Log("Spawn true gesetzt");
                s.canSpawnCoin = true;
                coinSpawnsActive = true;
            }
        }

        if(canFall)
            timer -= Time.deltaTime; // zählt Sekundenweise	

        //shake
        if (timer < warning && canFall && !gameEnded) {
            if(shakeRoutine == null)
                shakeRoutine = StartCoroutine(ShakePlatform());
        }

        if(timer > 0) {
            //moving up
            transform.position = -Vector3.up * Mathf.Min(timer * -1 + duration - appearTime, 0) * Mathf.Min(timer * -1 + duration - appearTime, 0) * fallSpeed;  
        } else {
            //moving down
            transform.position = -Vector3.up * Mathf.Min(timer, 0) * Mathf.Min(timer, 0) * fallSpeed;
        }

        if (timer < -appearTime) {
            //platform fell down and needs to be deactivated
            Remove();
        }

        isMoving = (timer < duration - appearTime && timer > warning) ? false : true;
        if(!isMoving && !platformSpawn.enablePlatformCycle){
            foreach(CoinSpawnPoint s in spawns){
                s.canSpawnCoin = true;
                coinSpawnsActive = true;
            }
            this.enabled = false;
        }
        
    }

    IEnumerator ShakePlatform(){
        foreach(CoinSpawnPoint s in spawns){
            //Debug.Log("Spawn false gesetzt");
            s.canSpawnCoin = false;
            coinSpawnsActive = false;
        }

        if(gameEnded) yield break;

        SetEdgeDirtPlaying(true);
        if (!playedDescendSound) {
            platformAudio.PlayOneShot(descendClips[(int)transform.rotation.y / 120]);
            playedDescendSound = true;
        }
        // Debug.Log("Platform " + gameObject.name + " starts tweens");
        shakeTween = Tween.Shake(transform, transform.position, shakiness, warning, 0f, Tween.LoopType.None, null, null, true);
        rotateTween = Tween.Rotate(transform, descendTilt, Space.Self, descendTiltTime, warning * 0.75f, Tween.EaseInOutBack, Tween.LoopType.None, null, null, true);
        yield return null;
        // while(gameObject.activeSelf){
        //     Vector3 offset = new Vector3(Random.Range(-shakiness, shakiness), Random.Range(-shakiness, shakiness), Random.Range(-shakiness, shakiness));

        //     for (int i = 0; i < transform.childCount; i++)
        //     {
        //         if(!transform.GetChild(i).name.Contains("edgeParticle"))
        //             transform.GetChild(i).localPosition = basePos[i] + offset;
        //     }
        // }
    }

    void Remove () {
        if(shakeRoutine != null){
            StopCoroutine(shakeRoutine);
            shakeRoutine = null;
        }
        transform.GetComponentInParent<PlatformSpawn>().newPlatform(currentPosition);
        currentPosition = -1;
        transform.position = new Vector3(0, -1000, 0);
        playedDescendSound = false;
        coinSpawnsActive = false;
        SetEdgeDirtPlaying(false);
        gameObject.SetActive(false);
    }

    //if a platform's coin is picked up, it immediately starts descend procedures
    public void CoinPickedUp() {
        canFall = true;
        timer = warning;
    }

    void OnWinCamActivated(){
        // Debug.Log("Platform " + gameObject.name + " noticed wincam");
        gameEnded = true;
        if(shakeTween != null) shakeTween.Cancel();
        if(rotateTween != null) rotateTween.Stop();
        SetEdgeDirtPlaying(false);
        canFall = false;
        platformAudio.Stop();
        platformAudio.volume = 0;
    }

    void SetEdgeDirtPlaying(bool playState)
    {
        if(playState && (!platformEdgeDirt[0].IsPlaying() || !platformEdgeDirt[1].IsPlaying()))
        {
            platformEdgeDirt[0].PlayParticleSystem();
            platformEdgeDirt[1].PlayParticleSystem();
        } else if(!playState && (platformEdgeDirt[0].IsPlaying() || platformEdgeDirt[1].IsPlaying()))
        {
            platformEdgeDirt[0].StopParticleSystem();
            platformEdgeDirt[1].StopParticleSystem();
        }
    }

    void OnDestroy(){
        PlayerManager.startWinCamEvent -= OnWinCamActivated;
    }

}
