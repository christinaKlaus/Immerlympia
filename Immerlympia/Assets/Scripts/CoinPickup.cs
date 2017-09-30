using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

    AudioSource coinSoundSource;
    public AudioClip spawn;

    private void Awake()
    {
        if(coinSoundSource == null)
            coinSoundSource = GetComponent<AudioSource>();

        coinSoundSource.PlayOneShot(spawn);
    }

    // Use this for initialization
    void Start () {
        InvokeRepeating("PosCheck", 0, 2);
        if (coinSoundSource == null)
            coinSoundSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y < -20) {
            Die();
            Debug.Log("Coin too low");
        }
	}

    void OnTriggerEnter(Collider collision) {
        GameObject obj = collision.gameObject;
        PlayerController pc = obj.GetComponent<PlayerController>();

        if(pc != null) {
            pc.GetComponent<SoundManager>().playClip(SoundType.Collect);
            pc.CoinCountUp();
            Die();
        }

    }

    public void Die () {
        CoinSpawnManager.coinActive = false;
        transform.parent.parent.GetComponent<PlatformScript>().canFall = true;
        Destroy(gameObject);
    }

    void PosCheck(){
        RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.down, out hit, 22)){
            transform.position = hit.point + Vector3.up;
        }else{
            Die();
        }
    }
}
