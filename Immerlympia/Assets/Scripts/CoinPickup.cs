using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision) {
        GameObject obj = collision.gameObject;
        Playercontroller pc = obj.GetComponent<Playercontroller>();

        if(pc != null) {
            pc.CoinCountUp();
            Destroy(gameObject);

            GameObject spawn = GameObject.Find("Spawn");
            if (spawn == null)
                Debug.Log("Spawn not found");
            spawn.GetComponent<CoinSpawn>().SpawnCoin();
        }

    }
}
