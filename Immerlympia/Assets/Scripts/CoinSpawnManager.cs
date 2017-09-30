using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawnManager : MonoBehaviour {
	public float nextSpawnTime;
	public static List<CoinSpawnPoint> possibleCoinSpawns;
	public static bool coinActive = false;
	private int index;

	// public CoinSpawnPoint[] coinArr;

	void Awake(){
		possibleCoinSpawns = new List<CoinSpawnPoint>();
	}

	void Start () {
		GameObject[] spawns = GameObject.FindGameObjectsWithTag("spawnPoint");
		foreach(GameObject g in spawns){
			if(!possibleCoinSpawns.Contains(g.GetComponent<CoinSpawnPoint>()) && !(g.transform.parent.GetComponent<PlatformScript>().movingUp)){
				possibleCoinSpawns.Add(g.GetComponent<CoinSpawnPoint>());
			}
		}
		
		// Debug.Log("poss spawns in start " + possibleCoinSpawns.Count);
	}

	void Update () {

		if(!coinActive){
			spawnNewCoin();
		}

		// coinArr = possibleCoinSpawns.ToArray();
		
	}

	void spawnNewCoin(){
		
		index = Random.Range(0, possibleCoinSpawns.Count);
		
		if(possibleCoinSpawns[index].canSpawnCoin){
			possibleCoinSpawns[index].spawnCoin();
			coinActive = true;
		}

	}
}
