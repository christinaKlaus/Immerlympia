using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class PressurePlateScript : MonoBehaviour {

    public float cooldown;
    public KnockBackWave waveGenerator;
    public LaserBeamScript laserBeamMount;

    [SerializeField] ParticleSystem activationParticles;
    [SerializeField] SimpleAudioEvent activationAudio;
    private CameraTurn cameraTurn;
    private AudioSource audioSource;
    string playerTag = "Player";


    public void Awake(){
        cameraTurn = GameObject.Find("CameraTurn").GetComponent<CameraTurn>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collider) {
        
        if(!collider.CompareTag(playerTag))
            return;
        
        //Collider coll = collider;
        //Debug.Log(coll);

        float angle = Mathf.Atan2(-transform.position.x, -transform.position.z) * Mathf.Rad2Deg;
        angle = Mathf.Round((angle - 30) / 120) * 120 + 30;
        
        //bool cameraNeedsTurn = 
        if(cameraTurn.StartRotation(angle)){
            activationParticles.Play(true);
            activationAudio.Play(audioSource);
        }

        //bool can be evaluated to disable waves when no cameraturn would be done on activation of pressure plate
        //if(cameraNeedsTurn)
        // waveGenerator.StartWave(collider.name);

        //laserBeamMount.StartLaser(collider.GetComponent<Transform>().position);

        /*
        for(int i = 0; i < PlayerManager.current.transform.childCount; i++){
            if(PlayerManager.current.transform.GetChild(i) != coll.gameObject.GetComponent<Transform>()){
                PlayerManager.current.transform.GetChild(i).GetComponent<Dummy>().Damage(Vector3.zero);
            }
        }
        */
    }

    
}
