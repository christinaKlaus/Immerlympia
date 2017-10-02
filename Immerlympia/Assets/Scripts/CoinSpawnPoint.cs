using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawnPoint : MonoBehaviour {
	public GameObject coin;
	public bool hasCoin;
	public bool canSpawnCoin = false;

	// void Start(){
	// 	if(CoinSpawnManager.possibleCoinSpawns != null)
	// 		CoinSpawnManager.possibleCoinSpawns.Add(this);
	// }

	public void SpawnCoin(){
		Instantiate(coin, transform.position, Quaternion.identity, transform.transform);
		hasCoin = true;
		canSpawnCoin = false;
		transform.parent.GetComponent<PlatformScript>().canFall = false;
	}

	void OnEnable(){
		if(CoinSpawnManager.possibleCoinSpawns != null)
			CoinSpawnManager.possibleCoinSpawns.Add(this);
	}

	void OnDisable(){
		//Destroy(transform.GetChild(0));
		CoinSpawnManager.possibleCoinSpawns.Remove(this);
	}

	public void DestroyYourCoin(){
		transform.GetChild(0).GetComponent<CoinPickup>().Collect();
		hasCoin = false;
		canSpawnCoin = true;
	}
}
