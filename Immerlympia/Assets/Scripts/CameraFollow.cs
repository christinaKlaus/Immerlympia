using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform player;
    public Vector3 offset;
	// Use this for initialization
	void Start () {
		if(player == null)
            player = GameObject.FindGameObjectWithTag("Respawn").transform;
        
    }

    // Update is called once per frame
    void Update () {
        transform.position = player.position + offset;
        transform.LookAt(player);
	}
}
