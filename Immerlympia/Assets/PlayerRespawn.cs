using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour {

	public static PlayerRespawn current;
	public float respawnTime;
	public float[] timers;
	Transform[] players;
	
	void Awake(){
		current = this;
	}
	// Use this for initialization
	void Start(){

		players = new Transform[transform.childCount];

		for(int i = 0; i < transform.childCount; i++){
			players[i] = PlayerManager.current.transform.GetChild(i);
		}
		
		timers = new float[transform.childCount];
		
		for(int j = 0; j < timers.Length; j++){
			timers[j] = respawnTime + 0.1f;
		}
		
	}

	void Update () {
		for(int k = 0; k < timers.Length; k++){
			timers[k]+= Time.deltaTime;
			if(!players[k].gameObject.activeSelf && timers[k] >= respawnTime){
				players[k].gameObject.SetActive(true);
				Debug.Log("Player Number" + players[k].GetComponent<PlayerController>().playerNumber + " = " + players[k].gameObject.activeSelf);
			}
		}
	}
}
