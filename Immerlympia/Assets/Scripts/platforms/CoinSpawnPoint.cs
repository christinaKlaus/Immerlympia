using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CoinSpawnPoint : MonoBehaviour {
	CoinSpawnManager coinSpawnManager;
	PlatformScript thisPlatform;
	public GameObject coin;
	public bool hasCoin;
	public bool canSpawnCoin = false;

	// void Start(){
	// 	if(CoinSpawnManager.possibleCoinSpawns != null)
	// 		CoinSpawnManager.possibleCoinSpawns.Add(this);
	// }

	public void SpawnCoin(){
		Instantiate(coin, transform.position + (Vector3.up * 1.5f), Quaternion.identity, transform.transform);
		hasCoin = true;
		canSpawnCoin = false;
		transform.parent.GetComponent<PlatformScript>().canFall = false;
	}

	void OnEnable(){
		if(!coinSpawnManager)
			coinSpawnManager = GetComponentInParent<CoinSpawnManager>();
		if(!thisPlatform)
			thisPlatform = GetComponentInParent<PlatformScript>();

		if(coinSpawnManager.possibleCoinSpawns != null)
			coinSpawnManager.possibleCoinSpawns.Add(this);
	}

	void OnDisable(){
		//Destroy(transform.GetChild(0));
		coinSpawnManager.possibleCoinSpawns.Remove(this);
	}

	public void DestroyYourCoin(){
		transform.GetChild(0).GetComponent<CoinPickup>().Collect();
		hasCoin = false;
		canSpawnCoin = true;
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(CoinSpawnPoint))]
public class CoinSpawnPointEditor : Editor {
	public override void OnInspectorGUI(){
		GUIStyle signPostStyle = new GUIStyle(GUI.skin.label);
		signPostStyle.richText = true;
		signPostStyle.alignment = TextAnchor.MiddleCenter;
		Rect signPostRect = EditorGUILayout.GetControlRect();
		EditorGUI.DrawRect(signPostRect, Color.yellow);
        EditorGUI.LabelField(signPostRect, "<color=red>Attention:</color> Place coin spawn point at ground level", signPostStyle);
		DrawDefaultInspector();
	}
}
#endif
