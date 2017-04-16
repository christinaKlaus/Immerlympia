using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {

    float stunned = 0;

    Rigidbody rigid;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (rigid == null)
            return;
        stunned -= Time.deltaTime;
        rigid.isKinematic = stunned > 0; 
        

	}

    public void damage(GameObject enemy) {
        
        if (rigid == null)
            return;
        Debug.Log("Au!" + gameObject.name + " | " + stunned);
        //Vector3 hitDir = transform.position - enemy.transform.position;
        if (stunned < -5) //5 Sek unverwundbar
        stunned = 2;

    }
}
