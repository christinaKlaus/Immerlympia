using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {

    public float knockback;
    public float stunTime;
    public float cooldown;

    float stunned = 0;
    PlayerController controller;
    
	// Use this for initialization
	void Start () {
        controller = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (controller == null)
            return;
        stunned -= Time.deltaTime;
        controller.canMove = stunned < 0; 
        

	}

    public void Damage(GameObject enemy) {

        if (stunned > -cooldown)
            return;

        if (controller == null) {
            Debug.Log("No Controller found");
            return;
        }
        
        Vector3 velocity = ((gameObject.transform.position - enemy.transform.position).normalized) * knockback;
        controller.velocityReal = velocity;

        stunned = stunTime;
    }
}
