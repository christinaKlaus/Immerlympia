﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCoinSpawn : MonoBehaviour {

	public GameObject coin;
	public float radius;
    public float minRadius;
    public float spawnHeight;
    public float startDistToLast;

    private Vector3 prevPos;
    private Vector3 pos;
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
        pos = Vector3.zero;

        for(int i = 0; i < 20; i++) {
            float t = 2 * Mathf.PI * Random.value;
            float u = Random.value + Random.value;
            float r = ((u > 1) ? 2 - u : u) * radius;
            pos = new Vector3(r * Mathf.Cos(t), spawnHeight, r * Mathf.Sin(t));
            goodPos = Physics.Raycast(pos, Vector3.down, spawnHeight);
            if (goodPos && r > minRadius && Vector3.Distance(pos, prevPos) > startDistToLast * (1 - i/10))
                break;
            goodPos = false;
            //Debug.Log("Pos: " + pos + " | goodPos: " + goodPos);
        }

        if (goodPos) {
            Invoke("InstantiateCoin", 1.5f);
            //Instantiate(coin, pos, transform.rotation);
            prevPos = pos;
        } else {
            timer = 1;
        }
        
	}

    void InstantiateCoin()
    {
        Instantiate(coin, pos, transform.rotation);
    }

   
}
