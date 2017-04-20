using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour {

	public GameObject coin;
	public float radius;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		SpawnCoin();
	}

	void SpawnCoin(){
		float t = 2 * Mathf.PI * Random.value;
		float u = Random.value + Random.value;
		float r = ((u>1) ? 2-u : u) * radius;
		Vector3 pos = new Vector3(r*Mathf.Cos(t), 20, r*Mathf.Sin(t)); 
		Instantiate(coin, pos, transform.rotation);
	}
}
