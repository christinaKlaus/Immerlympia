using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControlling : MonoBehaviour
{

    public delegate void UpdateScoreDelegate(int playerIndex, int score);
    public static event UpdateScoreDelegate UpdateScoreEvent;

    [HideInInspector] public int score = 0;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public Vector3 velocityReal = Vector3.zero;

    [HideInInspector] public float yVelocity;

    public int playerIndex;
    public float walkSpeed;
    public float acceleration;
    public float deceleration;

    public float jumpSpeed;
    public int maxJumps;
    [HideInInspector] public float timeOfLastDeath;

    private int jumps;
    private int timesJumped;
    private bool deathSoundPlayed = false;


    Animator anim;
    GameObject player;
    Camera cam;
    CharacterController charCon;
    SoundManager soundMan;
    PlayerManager playerManager;

    // Use this for initialization
    void Start()
    {
        player = gameObject;
        charCon = GetComponent<CharacterController>();
        cam = Camera.main;
        anim = GetComponent<Animator>();
        soundMan = GetComponent<SoundManager>();
        playerManager = GetComponentInParent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterDeath();
        movement();
        punch();
    }

    // Hitting hittable things (with Dummy script attached to them)
    private void punch()
    {
        if (Input.GetButtonDown("Punch" + playerIndex) && canMove)
        {
            //Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.magenta, 1, false);

            RaycastHit[] hit = Physics.CapsuleCastAll(transform.position + (Vector3.up * 1.5f), transform.position + (Vector3.up * 0.5f), 1.5f, transform.forward, 5.0f);
            anim.SetTrigger("punching");

            foreach (RaycastHit h in hit)
            {
                Dummy dummy = h.collider.GetComponent<Dummy>(); // Making sure the object can be hit

                if (dummy == null || h.collider.gameObject == gameObject) continue; // Object can not be hit

                dummy.Damage(gameObject); // Let the object hit itself
            }

        }
    }

    private void movement()
    {

        Vector3 velocityGoal = Vector3.zero;

        velocityReal.Scale(new Vector3(1, 0, 1));

        // <--- Basic movement --->

        if (canMove)
        {
            Vector3 forward = transform.position - cam.transform.position;
            forward.Scale(new Vector3(1, 0, 1));
            forward.Normalize();
            velocityGoal = forward * Input.GetAxis("Vertical" + playerIndex) * walkSpeed;

            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            right.Scale(new Vector3(1, 0, 1));
            right.Normalize();
            velocityGoal += right * Input.GetAxis("Horizontal" + playerIndex) * walkSpeed;

        }
        else
        {

            velocityGoal = Vector3.zero;
        }

        Vector3 velocityDiff = velocityGoal - velocityReal;

        float acc = velocityGoal == Vector3.zero ? deceleration : acceleration;

        if (velocityDiff.magnitude > acc * Time.deltaTime)
        {

            velocityReal += velocityDiff.normalized * acc * Time.deltaTime;
        }
        else
        {
            velocityReal = velocityGoal;
        }

        if (velocityReal != Vector3.zero)
            transform.LookAt(transform.position + velocityReal);

        // <---- Jumping ---->

        if (charCon.isGrounded)
        {
            jumps = maxJumps;
            timesJumped = 0;
            yVelocity = 0;
        }

        yVelocity += Physics.gravity.y * Time.deltaTime;

        if (Input.GetButtonDown("Jump" + playerIndex) && jumps > 0 && canMove)
        {
            yVelocity = (Mathf.Max(yVelocity, 0) + jumpSpeed);
            jumps--;
            timesJumped++;
            if (timesJumped > 1)
            {
                anim.SetTrigger("doubleJump");
                //Debug.Log("DoubleJump reached");
            }
        }

        velocityReal.y = yVelocity;
        //print(charCon.isGrounded);

        anim.SetBool("isJumping", !charCon.isGrounded && velocityReal.y > 0.5f);


        // <--- Setting velocity -->
        charCon.Move(velocityReal * Time.deltaTime);


        velocityReal.Scale(new Vector3(1, 0, 1));
        /*if(!soundMan.IsInvoking() && velocityReal.magnitude > 5 && charCon.isGrounded)
            soundMan.startFootsteps();

        if(velocityReal.magnitude <= 1 || !charCon.isGrounded)
            soundMan.stopFootsteps();*/

        anim.SetFloat("speed", Mathf.Sqrt(Mathf.Pow(charCon.velocity.x, 2) + Mathf.Pow(charCon.velocity.z, 2)));
        //if(anim.GetFloat("speed") != 0 && !GetComponent<AudioSource>().isPlaying)
        //  soundMan.playClip(SoundType.Steps);
    }

    public void CoinCountUp(int scoreIncrease)
    {
        ChangeScore(scoreIncrease);
    }

    public void CharacterDeath()
    {
        float posY = charCon.transform.position.y;

        if (posY < -50)
        {
            if (!deathSoundPlayed)
            {
                deathSoundPlayed = true;
                soundMan.playClip(SoundType.Death);
            }
            //Debug.Log("Death of Player" + playerIndex);
            ChangeScore(-1);
            player.transform.position = new Vector3(0, 10, 0);
            velocityReal = Vector3.zero;
            playerManager.CharacterDeath(playerIndex);
            timeOfLastDeath = Time.time;
        }
    }

    void ChangeScore(int scoreChange){
        score += scoreChange;
        if(UpdateScoreEvent != null)
            UpdateScoreEvent(playerIndex, score);
    }


    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "step":
                soundMan.playClip(SoundType.Steps);
                break;
            case "punch":
                soundMan.playClip(SoundType.Punch);
                break;
            case "jump":
                soundMan.playClip(SoundType.Jump);
                break;
            case "doubleJump":
                soundMan.playClip(SoundType.DoubleJump);
                break;
            default:
                break;
        }
    }
}
