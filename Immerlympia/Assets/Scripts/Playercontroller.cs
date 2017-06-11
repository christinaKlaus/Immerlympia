using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour {

    
    [System.NonSerialized] public int score = 0;

    public int playerNumber;
    public float walkSpeed;
    public float jumpSpeed;
    public int maxJumps;
    public float smooth; // 0-1 Range, wenn 1 -> 1 Sek bis max Geschwindigkeit

    private int jumps;
    private int timesJumped;
    private float smoothUp;
    private float airborne = 0;

    Animator anim;
    GameObject player;
    Camera cam;
    Rigidbody rigid;
       
    // bool isJumping = true;

	// Use this for initialization
	void Start () {
        player = gameObject;
        rigid = GetComponent<Rigidbody>();
        cam = Camera.current;
        anim = GetComponent<Animator>();
                
        /*
        switch (playerNumber) {
            case 0:
                transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.red;
                break;
            case 1:
                transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.blue;
                break;
            case 2:
                transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.green;
                break;
           case 3:
                transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.yellow;
                break;
        } */
    }
	
	// Update is called once per frame
	void Update () {
        // body.velocity = new Vector3(Input.GetAxis("Horizontal")*walkSpeed, body.velocity.y, Input.GetAxis("Vertical")*walkSpeed);
        if(smoothUp < 1 && (Input.GetAxis("Vertical" + playerNumber) != 0 || Input.GetAxis("Horizontal" + playerNumber) != 0)){
            smoothUp += smooth * Time.deltaTime; 
        }else{
            if(smoothUp > 0)
                smoothUp -= smooth * Time.deltaTime;
        }


        if (cam == null)
            cam = Camera.current;

        if (cam == null)
            return;

        CharacterDeath();
        movement();
        punch();
    }

    // Hitting hittable things (with Dummy script attached to them)
    private void punch() {
        if(Input.GetButtonDown("Punch" + playerNumber)) {
            //Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.magenta, 1, false);
            
            RaycastHit[] hit = Physics.CapsuleCastAll(transform.position + (Vector3.up * 1.5f), transform.position + (Vector3.up * 0.5f), 1.5f, transform.forward, 5.0f);
            anim.SetTrigger("punching");

            foreach (RaycastHit h in hit){ 
                Dummy dummy = h.collider.GetComponent<Dummy>(); // Making sure the object can be hit

                if (dummy == null || h.collider.gameObject == gameObject) continue; // Object can not be hit

                dummy.damage(gameObject); // Let the object hit itself
            }

        }
    }

    private void movement() {

        Vector3 velocity = Vector3.zero;
        
        // <--- Basic movement --->
        Vector3 forward = transform.position - cam.transform.position;
        forward.Scale(new Vector3(1, 0, 1));
        forward.Normalize();
        velocity = forward * Input.GetAxis("Vertical" + playerNumber) * walkSpeed * smoothUp;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        right.Scale(new Vector3(1, 0, 1));
        right.Normalize();
        velocity += right * Input.GetAxis("Horizontal" + playerNumber) * walkSpeed * smoothUp;

        CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();

        if (playerCollider == null)
            return;
        

        // <---- Jumping ---->
        
        //*
        RaycastHit hitGround;
        bool hitBool = Physics.Raycast(transform.position + transform.up, -transform.up, out hitGround, 1.1f);


        if (rigid.velocity.y <= 0 && hitBool) {
            jumps = maxJumps;
            timesJumped = 0;
        }

        airborne += Time.deltaTime;

        float yVelocity = rigid.velocity.y;

        if (Input.GetButtonDown("Jump" + playerNumber) && jumps > 0 && airborne > 0.1f) {
            yVelocity = (Mathf.Max(rigid.velocity.y, 0) + jumpSpeed);
            jumps--;
            timesJumped++;
            airborne = 0;
        }
        
        if(timesJumped > 1) {
            anim.SetTrigger("doubleJump");
            Debug.Log("doubleJump " + timesJumped);
        }
       
        anim.SetBool("isJumping", !hitBool);
        // Debug.DrawRay(transform.position,- transform.up);
       
        /*/
        if (Input.GetButtonDown("Jump" + playerNumber) && !isJumping) {
            velocity += (new Vector3(0,1,0)) * jumpSpeed;
            isJumping = true;
            Debug.Log(playerNumber);
        }
        //*/
        
        // <--- Setting velocity -->
        
        velocity.Scale(new Vector3(1, 0, 1));
        
        Vector3 direction = new Vector3(velocity.x, yVelocity, velocity.z);
        rigid.velocity = direction;

        anim.SetFloat("speed", velocity.magnitude);

        if (velocity != Vector3.zero)
           transform.LookAt(transform.position + velocity);
        
    }

    public void CoinCountUp() {
        score++;
        Debug.Log("Score Player" + playerNumber + " =     " + score);
    }
   
    public void CharacterDeath() {

        if (rigid.transform.position.y < -50) {
            Debug.Log("Death of Player" + playerNumber);
            player.transform.position = new Vector3(0, 10, 0);
            rigid.velocity = Vector3.zero;
        }
    }
}
