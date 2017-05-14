using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collider) {
        Transform plateTransform = transform.GetChild(0);
        float angle = Mathf.Atan2(-transform.position.x, -transform.position.z) * Mathf.Rad2Deg;
        angle = Mathf.Round((angle - 30) / 120) * 120 + 30;
        GameObject cam = GameObject.Find("CameraTurn");
        Debug.Log(cam);
        cam.GetComponent<CameraTurn>().StartRotation(angle);

    }

    
}
