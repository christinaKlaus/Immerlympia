using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CoinSpawnManager : MonoBehaviour {
	public float nextSpawnTime;
	public List<CoinSpawnPoint> possibleCoinSpawns;
	[ReadOnly(false)] public bool coinActive = false;

    private int index;
    private float timer; 
	[ReadOnly(false), SerializeField] private bool coinSpawnActive = true;

	// public CoinSpawnPoint[] coinArr;

	void Awake(){
		possibleCoinSpawns = new List<CoinSpawnPoint>();
	}

	void Start () {
		List<string> args = new List<string>(System.Environment.GetCommandLineArgs());
        if(args.Contains("-noCoins")) {
			coinSpawnActive = false;
		}

		GameObject[] spawns = GameObject.FindGameObjectsWithTag("spawnPoint");
		foreach(GameObject g in spawns){
			if(!possibleCoinSpawns.Contains(g.GetComponent<CoinSpawnPoint>()) && !(g.transform.parent.GetComponent<PlatformScript>().isMoving)){
				possibleCoinSpawns.Add(g.GetComponent<CoinSpawnPoint>());
			}
		}
		
		// Debug.Log("poss spawns in start " + possibleCoinSpawns.Count);
	}

	void Update () {
		if(coinSpawnActive){
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
		}
		
        // coinArr = possibleCoinSpawns.ToArray();
		if(Input.GetKey(KeyCode.LeftControl)){
			if(Input.GetKeyDown(KeyCode.C)){
				if(Input.GetKey(KeyCode.LeftShift)){
					coinSpawnActive = !coinSpawnActive;
				} else {
					SpawnNewCoinFromInspector();
				}
			}
		}

    }

	void SpawnNewCoin(){
		if(coinActive || possibleCoinSpawns.Count <= 0) return;
		
		index = Random.Range(0, possibleCoinSpawns.Count);
		
		if(possibleCoinSpawns[index].canSpawnCoin){
			possibleCoinSpawns[index].SpawnCoin();
			coinActive = true;
		}

	}

	public void SpawnNewCoinFromInspector(){
		if(coinActive || possibleCoinSpawns.Count <= 0) return;
		index = Random.Range(0, possibleCoinSpawns.Count);
		possibleCoinSpawns[index].SpawnCoin();
		coinActive = true;
	}

    public void ResetTimer()
    {
        timer = nextSpawnTime;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CoinSpawnManager))]
public class CoinSpawnManagerEditor : Editor {
	public override void OnInspectorGUI(){
		CoinSpawnManager coinSpawnManager = serializedObject.targetObject as CoinSpawnManager;
		DrawDefaultInspector();
		if(GUILayout.Button("Spawn Coin")){
			//Debug.Log("Spawn new coin button clicked");
			coinSpawnManager.SpawnNewCoinFromInspector();
		}
	}
}
#endif
