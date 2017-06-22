using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y < -50) {
            Die();
        }
	}

    void OnCollisionEnter(Collision collision) {
        GameObject obj = collision.gameObject;
        PlayerController pc = obj.GetComponent<PlayerController>();

        if(pc != null) {
            pc.CoinCountUp();
            Die();
        }

    }

    void Die () {
        Destroy(gameObject);

        GameObject spawn = GameObject.Find("Spawn");
        if (spawn == null)
            Debug.Log("Spawn not found");
        spawn.GetComponent<CoinSpawn>().SpawnCoin();
    }
}
