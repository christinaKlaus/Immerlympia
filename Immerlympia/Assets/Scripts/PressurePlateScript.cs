using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour {

    private void OnTriggerEnter(Collider collider) {
        
        Collider coll = collider;

        Debug.Log(coll);

        float angle = Mathf.Atan2(-transform.position.x, -transform.position.z) * Mathf.Rad2Deg;
        angle = Mathf.Round((angle - 30) / 120) * 120 + 30;
        
        GameObject cam = GameObject.Find("CameraTurn");
        cam.GetComponent<CameraTurn>().StartRotation(angle);

        for(int i = 0; i < PlayerManager.current.transform.childCount; i++){
            if(PlayerManager.current.transform.GetChild(i) != coll.gameObject.GetComponent<Transform>()){
                PlayerManager.current.transform.GetChild(i).GetComponent<Dummy>().Damage(Vector3.zero);
            }
        }

    }

    
}
