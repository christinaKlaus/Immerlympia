using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class CoinSpawnPoint : MonoBehaviour {
	
	public GameObject coin;
	public bool hasCoin;
	public bool canSpawnCoin = false;

	private bool isOnSceneEditPlatform = false;

	static CoinSpawnManager coinSpawnManager;
	PlatformScript thisPlatform;
	AudioSource audioSource;
	[SerializeField] AudioClip spawnSound, collectSound;

	void Awake(){
		if(coinSpawnManager == null){
			coinSpawnManager = FindObjectOfType<CoinSpawnManager>();
		}
		thisPlatform = GetComponentInParent<PlatformScript>();
		audioSource = GetComponent<AudioSource>();
	}

	public void SpawnCoin(CoinPickup coin){
		coin.transform.SetParent(transform);
		coin.transform.position = transform.position + (Vector3.up * 1.5f);
		coin.transform.rotation = Quaternion.identity;
		// Instantiate(coin, transform.position + (Vector3.up * 1.5f), Quaternion.identity, transform.transform);
		hasCoin = true;
		canSpawnCoin = false;
		thisPlatform.canFall = false;
		coin.Setup(thisPlatform, this);
		coin.gameObject.SetActive(true);
		audioSource.PlayOneShot(spawnSound);
	}

	/// <summary>Plays the coin collect sound.</summary>
	public void CoinCollected(){
		audioSource.PlayOneShot(collectSound);
	}

	void OnEnable(){
		if(GetComponentInParent<DeactivateInPlayMode>() != null) {
			isOnSceneEditPlatform = true;
			return;
		}
		if(coinSpawnManager.possibleCoinSpawns != null)
			coinSpawnManager.possibleCoinSpawns.Add(this);
	}

	void OnDisable(){
		if(isOnSceneEditPlatform) return;
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
