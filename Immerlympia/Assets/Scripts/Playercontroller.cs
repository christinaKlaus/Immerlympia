using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour {

    GameObject player;
    Camera cam;
    Rigidbody body;
    public float walkSpeed;
    public float jumpSpeed;
    bool isJumping = true;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        body = GetComponent<Rigidbody>();
        cam = Camera.current;
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
        if(Input.GetButtonDown("Fire1")) {
            Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.magenta, 1, false);

            RaycastHit hit;
            if(Physics.CapsuleCast(transform.position + (Vector3.up * 1.5f), transform.position + (Vector3.up * 0.5f), 0.5f, transform.forward, out hit, 1.5f)) {
                Dummy dummy = hit.collider.GetComponent<Dummy>(); //making sure the object can be hit
                if (dummy == null)
                    return;
                dummy.damage(gameObject); //let the object hit itself
            }

        }
    }

    private void movement() {
        Vector3 velocity = body.velocity;
        velocity.Scale(new Vector3(0, 1, 0));

        Vector3 forward = cam.transform.forward;
        forward.Scale(new Vector3(1, 0, 1));
        forward.Normalize();
        velocity += forward * Input.GetAxis("Vertical") * walkSpeed;

        Vector3 right = cam.transform.right;
        right.Scale(new Vector3(1, 0, 1));
        right.Normalize();
        velocity += right * Input.GetAxis("Horizontal") * walkSpeed;

        CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();

        if (playerCollider == null)
            return;

        //RaycastHit hitGround;
        //if (Input.GetButtonDown("Jump") && Physics.Raycast(transform.position, -transform.up, out hitGround, 1.1f))
        //   velocity.y = jumpSpeed;

        if (Input.GetButtonDown("Jump") && !isJumping) {
            velocity.y = jumpSpeed;
            isJumping = true;
        }

        body.velocity = velocity;
        velocity.Scale(new Vector3(1, 0, 1));

        if (velocity != Vector3.zero)
           transform.LookAt(transform.position + velocity);
        
    }

    private void OnCollisionEnter(Collision collision) {
        //Collison with ground
        if (collision.gameObject.tag == "Ground") {
            isJumping = false;
            
        } else {
            isJumping = true;
        }
        
    }

    public void CharacterDeath() {

        if (body.transform.position.y < 0) {
            Debug.Log("Death");
            player.transform.position = new Vector3(0, 10, 0); 

        }
    }
}
