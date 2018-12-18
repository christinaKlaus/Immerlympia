using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

    public AudioClip[] descendClips;
    public Vector2 lifetimeRange;
    public float fallSpeed;
    public float warning;
    public float shakiness;
    public float appearTime;

    public bool isMoving = true;
    
    [HideInInspector] public bool canFall = true;

    private float timer = 8;
    private float duration;
    private bool playedDescendSound = false;
    private bool spawnPointsActive = false;

    private Vector3[] basePos;
    private AudioSource platformAudio;
    private CoinSpawnPoint[] spawns;
    private platformDirtParticleSystem[] platformEdgeDirt;
    private PlatformSpawn platformSpawn;

    

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

        Update();
    }

    void Update () {
                
        if(!isMoving && !spawnPointsActive){
            foreach(CoinSpawnPoint s in spawns){
                // Debug.Log("Spawn true gesetzt");
                s.canSpawnCoin = true;
            }
        }

        if(canFall)
            timer -= Time.deltaTime; // zählt Sekundenweise	

        //shake
        if (timer < warning) {

            foreach(CoinSpawnPoint s in spawns){
                //Debug.Log("Spawn false gesetzt");
                s.canSpawnCoin = false;
            }

            SetEdgeDirtPlaying(true);

            if (!playedDescendSound) {
                platformAudio.PlayOneShot(descendClips[(int)transform.rotation.y / 120]);
                playedDescendSound = true;
            }

            if (basePos == null) {
                basePos = new Vector3[transform.childCount];
                for (int i = 0; i < transform.childCount; i++)
                    basePos[i] = transform.GetChild(i).localPosition;
            }

            Vector3 offset = new Vector3(Random.Range(-shakiness, shakiness), Random.Range(-shakiness, shakiness), Random.Range(-shakiness, shakiness));

            for (int i = 0; i < transform.childCount; i++)
            {
                if(!transform.GetChild(i).name.Contains("edgeParticle"))
                    transform.GetChild(i).localPosition = basePos[i] + offset;
            }
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
            this.enabled = false;
        }
        
    }

    void Remove () {
        transform.GetComponentInParent<PlatformSpawn>().newPlatform((int)transform.rotation.eulerAngles.y / 120);
        transform.position = new Vector3(0, -1000, 0);
        playedDescendSound = false;
        spawnPointsActive = false;
        SetEdgeDirtPlaying(false);
        gameObject.SetActive(false);
    }

    //if a platform's coin is picked up, it immediately starts descend procedures
    public void CoinPickedUp()
    {
        canFall = true;
        timer = warning;
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

}
