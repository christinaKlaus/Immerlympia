using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour {

	public GameObject coin;
	public float radius;
    public float minRadius;
    public float spawnHeight;

    private float timer;
	// Use this for initialization
	void Start () {
        SpawnCoin();
    }
	
	// Update is called once per frame
	void Update () {
        if(timer > 0) {
            timer -= Time.deltaTime;
            if(timer < 0) {
                SpawnCoin();
            }
        }

    }

	public void SpawnCoin(){
        bool goodPos = false;
        Vector3 pos = Vector3.zero;

        for(int i = 0; i < 20; i++) {
            float t = 2 * Mathf.PI * Random.value;
            float u = Random.value + Random.value;
            float r = ((u > 1) ? 2 - u : u) * radius;
            pos = new Vector3(r * Mathf.Cos(t), spawnHeight, r * Mathf.Sin(t));
            goodPos = Physics.Raycast(pos, Vector3.down, spawnHeight);
            if (goodPos && r > minRadius)
                break;
            goodPos = false;
            Debug.Log("Pos: " + pos + " | goodPos: " + goodPos);
        }

        if (goodPos) {
            Instantiate(coin, pos, transform.rotation);
        } else {
            timer = 1;
        }
        
	}
}
