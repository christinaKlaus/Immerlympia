using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour {

    Camera cam;
    Rigidbody body;
    public float walkSpeed;
    public float jumpSpeed;

	// Use this for initialization
	void Start () {
        Debug.Log("ich starte");
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

        movement();
        punch();
    }

    private void punch() {
        if(Input.GetButtonDown("Fire1")) {
            Debug.DrawRay(transform.position, transform.forward, Color.magenta, 1, false);

            RaycastHit hit;
            if(Physics.CapsuleCast(transform.position + (Vector3.up * 0.5f), transform.position - (Vector3.up * 0.5f), 0.5f, transform.forward, out hit, 1)) {
                Dummy dummy = hit.collider.GetComponent<Dummy>();
                if (dummy == null)
                    return;
                dummy.damage(gameObject);
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

        RaycastHit hit;
        if (Input.GetButtonDown("Jump") && Physics.Raycast(transform.position, -transform.up, out hit, 1.1f))
            velocity.y = jumpSpeed;

        body.velocity = velocity;
        velocity.Scale(new Vector3(1, 0, 1));

        if (velocity != Vector3.zero)
            transform.LookAt(transform.position + velocity);
        
    }

}
