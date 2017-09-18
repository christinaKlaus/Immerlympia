using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InvokeRepeating("PosCheck", 0, 2);
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y < -50) {
            Die();
        }
	}

    void OnTriggerEnter(Collider collision) {
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

    void PosCheck(){
        RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.down, out hit, 22)){
            transform.position = hit.point + Vector3.up;
        }else{
            Die();
        }
    }
}
