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
    public bool movingUp = true;
    [HideInInspector] public bool canFall = true;

    private float timer = 8;
    private float duration;
    private bool playedDescendSound = false;
    private bool spawnsAvailable = false;

    private Vector3[] basePos;
    private AudioSource platformAudio;
    private CoinSpawnPoint[] spawns;
    

    void OnEnable () {
        timer = Random.Range(lifetimeRange.x, lifetimeRange.y);
        duration = timer;
        spawns = GetComponentsInChildren<CoinSpawnPoint>();
        
        if(descendClips.Length == 0) {
            Debug.Log("Platform prefab " + gameObject.name + " is missing descend sounds");
        }
        
        if(platformAudio == null) {
            platformAudio = GetComponent<AudioSource>();
        }

        Update();
    }

    void Update () {
                
        if(!movingUp && !spawnsAvailable){
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
                CoinSpawnManager.possibleCoinSpawns.Remove(s);
            }

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
                transform.GetChild(i).localPosition = basePos[i] + offset;
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

        movingUp = (timer < duration - appearTime) ? false : true;

        
    }

    void Remove () {
        transform.GetComponentInParent<PlatformSpawn>().newPlatform((int)transform.rotation.eulerAngles.y / 120);
        transform.position = new Vector3(0, -1000, 0);
        gameObject.SetActive(false);
        playedDescendSound = false;
    }

}
