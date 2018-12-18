using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

    int scoreIncrease = 1;
    AudioSource coinSoundSource;
    public AudioClip spawn;

    private void Awake()
    {
        if(coinSoundSource == null)
            coinSoundSource = GetComponent<AudioSource>();

        coinSoundSource.PlayOneShot(spawn);
    }

	// Update is called once per frame
	void Update () {
		if(transform.position.y < -20) {
            CoinSpawnManager.coinActive = false;
            Destroy(gameObject);
            //Debug.Log("Coin too low");
        }
	}

    void OnTriggerEnter(Collider collision) {
        GameObject obj = collision.gameObject;
        PlayerControlling pc = obj.GetComponent<PlayerControlling>();

        if(pc != null) {
            pc.GetComponent<SoundManager>().playClip(SoundType.Collect);
            pc.CoinCountUp(scoreIncrease);
            Collect();
        }

    }

    public void Collect () {
        CoinSpawnManager.ResetTimer();
        CoinSpawnManager.coinActive = false;
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
