using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {

    public float knockback;
    public float stunTime;
    public float cooldown;
    public float vertKnockup;

    float stunned = 0;
    PlayerController controller;
    SoundManager soundMan;
    
	// Use this for initialization
	void Start () {
        controller = GetComponent<PlayerController>();
        soundMan = GetComponent<SoundManager>();
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

        Vector3 velocity = (gameObject.transform.position - enemyPos).normalized * knockback;
        controller.velocityReal = velocity;
        controller.yVelocity = vertKnockup;

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
        
        Vector3 velocity = (gameObject.transform.position - enemy.transform.position).normalized * knockback;
        controller.velocityReal = velocity;
        controller.yVelocity = vertKnockup;

        stunned = stunTime;
        
        Animator anim = this.gameObject.GetComponent<Animator>();
        anim.SetTrigger("getHit");
        soundMan.playClip(SoundType.Hit);
    }

    public void DamageByWave()
    {
        if (stunned > -cooldown)
            return;

        if (controller == null)
        {
            Debug.Log("No Controller found");
            return;
        }

        Vector3 velocity = (gameObject.transform.position - Vector3.zero).normalized * knockback;
        controller.velocityReal = velocity;
        controller.yVelocity = 10f;

        stunned = stunTime;

        Animator anim = this.gameObject.GetComponent<Animator>();
        anim.SetTrigger("getHit");
        soundMan.playClip(SoundType.Hit);
    }
}
