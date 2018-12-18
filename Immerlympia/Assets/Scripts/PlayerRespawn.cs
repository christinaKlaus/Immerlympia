using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerRespawn : MonoBehaviour {

	public static PlayerRespawn current;
	public float respawnTime;
	[HideInInspector] public Transform[] players;
	
	public UnityEvent stopTimerEvent;
	/*
	void Awake(){
		current = this;
	}
	// Use this for initialization
	void Start(){

		players = new Transform[transform.childCount];

		for(int i = 0; i < transform.childCount; i++){
			players[i] = PlayerManager.current.transform.GetChild(i);
		}
		
		
	}

	void Update () {
		for(int k = 0; k < timers.Length; k++){
			timers[k]-= Time.deltaTime;
			if(!players[k].gameObject.activeSelf && timers[k] <= 0){
				players[k].gameObject.SetActive(true);
				stopTimerEvent.Invoke();
			}
		}
	}
	*/
}
