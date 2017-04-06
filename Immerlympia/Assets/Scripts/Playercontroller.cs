using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour {

    public int playerNumber;

    GameObject player;
    Camera cam;
    Rigidbody body;
    public float walkSpeed;
    public float jumpSpeed;
    bool isJumping = true;

	// Use this for initialization
	void Start () {
        player = gameObject;
        body = GetComponent<Rigidbody>();
        cam = Camera.current;

        switch (playerNumber) {
            case 0:
                transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
                break;
            case 1:
                transform.GetChild(0).GetComponent<Renderer>().material.color = Color.blue;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //body.velocity = new Vector3(Input.GetAxis("Horizontal")*walkSpeed, body.velocity.y, Input.GetAxis("Vertical")*walkSpeed);
        
        if (cam == null)
            cam = Camera.current;

        if (cam == null)
            return;

        CharacterDeath();
        movement();
        punch();
    }

    //for hitting hittable things (with Dummy script attached to them)
    private void punch() {
        if(Input.GetButtonDown("Punch" + playerNumber)) {
            Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.magenta, 1, false);

            RaycastHit[] hit = Physics.CapsuleCastAll(transform.position + (Vector3.up * 1.5f), transform.position + (Vector3.up * 0.5f), 1.5f, transform.forward, 5.0f);
            foreach(RaycastHit h in hit){ 
                Dummy dummy = h.collider.GetComponent<Dummy>(); //making sure the object can be hit
                if (dummy == null || h.collider.gameObject == gameObject)
                    continue;
                dummy.damage(gameObject); //let the object hit itself
            }

        }
    }

    private void movement() {
        Vector3 velocity = body.velocity;
        velocity.Scale(new Vector3(0, 1, 0));

        Vector3 forward = transform.position - cam.transform.position;
        forward.Scale(new Vector3(1, 0, 1));
        forward.Normalize();
        velocity += forward * Input.GetAxis("Vertical" + playerNumber) * walkSpeed;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        right.Scale(new Vector3(1, 0, 1));
        right.Normalize();
        velocity += right * Input.GetAxis("Horizontal" + playerNumber) * walkSpeed;

        CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();

        if (playerCollider == null)
            return;

        //*
        RaycastHit hitGround;
        if (Input.GetButtonDown("Jump" + playerNumber) && Physics.Raycast(transform.position + transform.up, -transform.up, out hitGround, 1.1f))
           velocity.y = jumpSpeed;
        Debug.DrawRay(transform.position,- transform.up);
        /*/
        if (Input.GetButtonDown("Jump" + playerNumber) && !isJumping) {
            velocity += (new Vector3(0,1,0)) * jumpSpeed;
            isJumping = true;
            Debug.Log(playerNumber);
        }
        //*/
        
        body.velocity = velocity;
        velocity.Scale(new Vector3(1, 0, 1));

        if (velocity != Vector3.zero)
           transform.LookAt(transform.position + velocity);
        
    }

    //private void OnCollisionEnter(Collision collision) {
    //    Collison with ground
    //    if (collision.gameObject.tag == "Ground") {
    //        isJumping = false;
            
    //    }
        
    //}

    public void CharacterDeath() {

        if (body.transform.position.y < 0) {
            Debug.Log("Death");
            player.transform.position = new Vector3(0, 10, 0); 

        }
    }
}
