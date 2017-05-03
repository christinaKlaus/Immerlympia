using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurn : MonoBehaviour {

    float timer = 1000000;
    public float journeyTime;

    float start;
    float end;
    
    int targetPos;
    // Use this for initialization
    void Start() {
        start = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;
        //Debug.Log(timer);
        if (timer < journeyTime) {
            float angle = Mathf.LerpAngle(start, end, timer / journeyTime);
            Debug.Log(angle);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        
        
    }
       
    public void StartRotation(float targetAngle) {
        start = transform.eulerAngles.y;
        end = targetAngle;
        timer = 0;       
    }

}
