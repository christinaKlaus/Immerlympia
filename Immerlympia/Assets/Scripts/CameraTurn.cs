using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurn : MonoBehaviour {

    float timer = 1000000;
    public float journeyTime;
    public AudioClip turnClipLeft;
    public AudioClip turnClipRight;

    bool turnEnded = true;
    float start;
    float end;
    int lastPosition;
    AudioSource cameraAudioSource;
    
    int targetPos;
    // Use this for initialization
    void Start() {
        start = transform.eulerAngles.y;
        lastPosition = (int)start;
        //Debug.Log("Start angle = " + lastPosition);
        cameraAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;
        //Debug.Log(timer);
        if (timer < journeyTime) {
            float angle = Mathf.LerpAngle(start, end, timer / journeyTime);

            transform.rotation = Quaternion.Euler(0, angle, 0);
        } else if (!turnEnded)
        {
            transform.rotation = Quaternion.Euler(0, end, 0);
            turnEnded = true;
        }
    }
       
    public bool StartRotation(float targetAngle) {
        if ((int)targetAngle == lastPosition || (lastPosition == 270 && (int)targetAngle == -90))
            return false;

        start = transform.eulerAngles.y;
        end = targetAngle;
        timer = 0;
        //Debug.Log(lastPosition + "(" + start + ") to " + targetAngle);
        bool isRight = GetRotateDirection(lastPosition, end);
        if (!isRight)
        {
            cameraAudioSource.PlayOneShot(turnClipLeft);
            //Debug.Log("Left.");
        }
        else
        {
            cameraAudioSource.PlayOneShot(turnClipRight);
            //Debug.Log("Right");
        }
        turnEnded = false;
        lastPosition = (int)end;
        return true;
    }

    bool GetRotateDirection(float from, float to)
    {
        switch ((int)from)
        {
            case 270:
                return (to == -210 || to == 150) ? true : false;
            case -90:
                return (to == -210 || to == 150) ? true : false;
            case 150:
                return (to == 30) ? true : false;
            case -210:
                return (to == 30) ? true : false;
            case 30:
                return (to == -90 || to == 270) ? true : false;
            default:
                return true;
        }
    }
}
