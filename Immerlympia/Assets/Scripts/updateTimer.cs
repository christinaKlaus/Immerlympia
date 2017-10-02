using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateTimer : MonoBehaviour {

	private int index;
    Text text;

    private void Start() {
        index = transform.GetSiblingIndex();
        text = GetComponent<Text>();
        PlayerManager.current.players[index].startRespawnTimerEvent.AddListener(StartTimer);    
		PlayerRespawn.current.stopTimerEvent.AddListener(StopTimer);   
    }

    void StartTimer() {
		InvokeRepeating("UpdateTimer", 0, 0.001f);
    }

	void UpdateTimer(){
		text.text = "" + (int) PlayerRespawn.current.timers[index];
	}

	void StopTimer(){
		CancelInvoke("UpdateTimer");
		text.text = "";
	}
}
