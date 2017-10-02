using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawnManager : MonoBehaviour {
	public static float nextSpawnTime;
	public static List<CoinSpawnPoint> possibleCoinSpawns;
	public static bool coinActive = false;

    private int index;
    private static float timer; 

	// public CoinSpawnPoint[] coinArr;

	void Awake(){
		possibleCoinSpawns = new List<CoinSpawnPoint>();
	}

	void Start () {
		GameObject[] spawns = GameObject.FindGameObjectsWithTag("spawnPoint");
		foreach(GameObject g in spawns){
			if(!possibleCoinSpawns.Contains(g.GetComponent<CoinSpawnPoint>()) && !(g.transform.parent.GetComponent<PlatformScript>().movingUpOrDown)){
				possibleCoinSpawns.Add(g.GetComponent<CoinSpawnPoint>());
			}
		}
		
		// Debug.Log("poss spawns in start " + possibleCoinSpawns.Count);
	}

	void Update () {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        } else
        {
            if (!coinActive)
            {
                SpawnNewCoin();
            }
        }
        // coinArr = possibleCoinSpawns.ToArray();

    }

	void SpawnNewCoin(){
		
		index = Random.Range(0, possibleCoinSpawns.Count);
		
		if(possibleCoinSpawns[index].canSpawnCoin){
			possibleCoinSpawns[index].SpawnCoin();
			coinActive = true;
		}

	}

    public static void ResetTimer()
    {
        timer = nextSpawnTime;
    }
}
