using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

    int scoreIncrease = 1;
    AudioSource coinSoundSource;
    CoinSpawnManager coinSpawnManager;
    PlatformScript currentPlatform;
    public AudioClip spawn;

    void Awake()
    {
        coinSpawnManager = GetComponentInParent<CoinSpawnManager>();
        currentPlatform = GetComponentInParent<PlatformScript>();
        if(coinSoundSource == null)
            coinSoundSource = GetComponent<AudioSource>();

        coinSoundSource.PlayOneShot(spawn);
    }

	// Update is called once per frame
	void Update () {
		if(transform.position.y < -20) {
            coinSpawnManager.coinActive = false;
            currentPlatform.canFall = true;
            Destroy(gameObject);
            //Debug.Log("Coin too low");
        }
	}

    void OnTriggerEnter(Collider collision) {
        GameObject obj = collision.gameObject;
        PlayerControlling pc = obj.GetComponent<PlayerControlling>();

        if(pc != null) {
            pc.GetComponent<SoundManager>().playClip(SoundType.Collect);
            pc.ChangeScore(scoreIncrease);
            Collect();
        }

    }

    public void Collect () {
        coinSpawnManager.ResetTimer();
        coinSpawnManager.coinActive = false;
        transform.parent.parent.GetComponent<PlatformScript>().CoinPickedUp();
        Destroy(gameObject);
    }

    /*void PosCheck(){
        RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.down, out hit, 22)){
            transform.position = hit.point + Vector3.up;
        }else{
            Die();
        }
    }*/
}
