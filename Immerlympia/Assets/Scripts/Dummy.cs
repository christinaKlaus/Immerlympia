using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {

    public float knockback;
    public float stunTime;
    public float cooldown;
    public float vertKnockup;

    private Rigidbody rigid;
    float stunned = 0;
    PlayerController controller;
    
	// Use this for initialization
	void Start () {
        controller = GetComponent<PlayerController>();
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (controller == null)
            return;
        stunned -= Time.deltaTime;
        controller.canMove = stunned < 0; 

	}

    public void Damage(Vector3 enemyPos){
         if (controller == null) {
            Debug.Log("No Controller found");
            return;
        }

        Vector3 velocity = (gameObject.transform.position - enemyPos.normalized) * knockback;
        controller.velocityReal = velocity;
        rigid.velocity += Vector3.up * vertKnockup; 

        stunned = stunTime;
        
        Animator anim = this.gameObject.GetComponent<Animator>();
        anim.SetTrigger("getHit");
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
        rigid.velocity += Vector3.up * vertKnockup; 

        stunned = stunTime;
        
        Animator anim = this.gameObject.GetComponent<Animator>();
        anim.SetTrigger("getHit");
    }
}
