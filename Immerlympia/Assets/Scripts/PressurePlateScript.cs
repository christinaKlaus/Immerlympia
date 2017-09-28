using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour {

    public float cooldown;

    float timer;
    public KnockBackWave waveGenerator;
    public LaserBeamScript laserBeamMount;

    void Start(){
        timer = cooldown;
        waveGenerator = GameObject.FindGameObjectWithTag("waveGenerator").GetComponent<KnockBackWave>();
        //laserBeamMount = GameObject.FindGameObjectWithTag("laserBeamMount").GetComponent<LaserBeamScript>();
    }

    void Update(){
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider) {
        
        if(timer < cooldown || collider.tag != "Player")
            return;
        
        //Collider coll = collider;

        //Debug.Log(coll);

        float angle = Mathf.Atan2(-transform.position.x, -transform.position.z) * Mathf.Rad2Deg;
        angle = Mathf.Round((angle - 30) / 120) * 120 + 30;
        
        GameObject cam = GameObject.Find("CameraTurn");
        bool cameraNeedsTurn = cam.GetComponent<CameraTurn>().StartRotation(angle);

        //bool can be evaluated to disable waves when no cameraturn would be done on activation of pressure plate
        //if(cameraNeedsTurn)
        waveGenerator.StartWave(collider.name);

        //laserBeamMount.StartLaser(collider.GetComponent<Transform>().position);

      /*  for(int i = 0; i < PlayerManager.current.transform.childCount; i++){
            if(PlayerManager.current.transform.GetChild(i) != coll.gameObject.GetComponent<Transform>()){
                PlayerManager.current.transform.GetChild(i).GetComponent<Dummy>().Damage(Vector3.zero);
            }
        }
       */
        timer = 0;
    }

    
}
