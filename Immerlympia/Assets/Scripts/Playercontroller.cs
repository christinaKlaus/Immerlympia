using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour {

    
    [System.NonSerialized] public int score = 0;
    [System.NonSerialized] public bool canMove = true;
    [System.NonSerialized] public Vector3 velocityReal = Vector3.zero;
    [System.NonSerialized] public int playerNumber;

    public float walkSpeed;
    public float acceleration;
    public float jumpSpeed;
    public int maxJumps;
    

    private int jumps;
    private int timesJumped;
    private float airborne = 0;
    

    Animator anim;
    GameObject player;
    Camera cam;
    Rigidbody rigid;

    public UnityEvent addScore;

	// Use this for initialization
	void Start () {
        player = gameObject;
        rigid = GetComponent<Rigidbody>();
        cam = Camera.main;
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (cam == null)
            cam = Camera.main;

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

                dummy.Damage(gameObject); // Let the object hit itself
            }

        }
    }

    private void movement() {

        Vector3 velocityGoal = Vector3.zero;
        
        velocityReal.Scale(new Vector3(1, 0, 1));

        // <--- Basic movement --->

        if (canMove) {
            Vector3 forward = transform.position - cam.transform.position;
            forward.Scale(new Vector3(1, 0, 1));
            forward.Normalize();
            velocityGoal = forward * Input.GetAxis("Vertical" + playerNumber) * walkSpeed;

            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            right.Scale(new Vector3(1, 0, 1));
            right.Normalize();
            velocityGoal += right * Input.GetAxis("Horizontal" + playerNumber) * walkSpeed;

        } else {

            velocityGoal = Vector3.zero;
        }

        Vector3 velocityDiff = velocityGoal - velocityReal;
        
        if(velocityDiff.magnitude > acceleration * Time.deltaTime) {
            velocityReal += velocityDiff.normalized * acceleration * Time.deltaTime;
        } else {
            velocityReal = velocityGoal;
        }
        
        if (velocityReal != Vector3.zero)
            transform.LookAt(transform.position + velocityReal);

        // <---- Jumping ---->
        RaycastHit hitGround;
        bool hitBool = Physics.Raycast(transform.position + transform.up, -transform.up, out hitGround, 1.1f);
        
        if (rigid.velocity.y <= 0 && hitBool) {
            jumps = maxJumps;
            timesJumped = 0;
        }

        airborne += Time.deltaTime;

        velocityReal.y = rigid.velocity.y;

        if (Input.GetButtonDown("Jump" + playerNumber) && jumps > 0 && airborne > 0.1f) {
            velocityReal.y = (Mathf.Max(rigid.velocity.y, 0) + jumpSpeed);
            jumps--;
            timesJumped++;
            airborne = 0;
        }
        
        if(timesJumped > 1) {
            anim.SetTrigger("doubleJump");
        }
       
        anim.SetBool("isJumping", !hitBool);


        // <--- Setting velocity -->
        rigid.velocity = velocityReal;


        anim.SetFloat("speed", rigid.velocity.magnitude);

    }

    public void CoinCountUp() {
        score++;
        addScore.Invoke();
    }
   
    public void CharacterDeath() {

        if (rigid.transform.position.y < -50) {
            Debug.Log("Death of Player" + playerNumber);
            player.transform.position = new Vector3(0, 10, 0);
            rigid.velocity = Vector3.zero;
        }
    }
}
