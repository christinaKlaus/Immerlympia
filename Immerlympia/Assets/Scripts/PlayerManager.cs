using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public PlayerController[] players;
    public Material[] playerMaterials;
    public static PlayerManager current;

    void Awake() {
        current = this;    
        players = new PlayerController[transform.childCount];
        for (int i = 0; i < players.Length; i++) {
            players[i] = transform.GetChild(i).GetComponent<PlayerController>();
            players[i].playerNumber = i;
            players[i].GetComponentInChildren<Renderer>().material = playerMaterials[i];
        }
        
    }
}
