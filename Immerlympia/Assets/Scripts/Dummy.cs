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

    public void Damage(GameObject enemy) {



        Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
        if (rigid == null) {
            Debug.Log("No Rigidbody found");
            return;
        }
        Debug.Log("Au!" + gameObject.name);
        //Debug.Log((gameObject.transform.position - enemy.transform.position).normalized);
        Vector3 velocity = ((gameObject.transform.position - enemy.transform.position).normalized) * 100;
        Debug.Log(velocity);
        rigid.velocity = velocity;

        if (stunned < -5) //5 Sek unverwundbar
        stunned = 2;


    }
}
