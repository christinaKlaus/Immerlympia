using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamScript : MonoBehaviour {

    public float journeyTime;

    bool beamActive = false;

    float startAngleY;
    float nextAngleY;
    float timer = float.MaxValue;

    Vector3 lastLookAtPosition;
    ParticleSystem laserBeam;
    LineRenderer laserLine;
    Quaternion fromToRotation;

    // Use this for initialization
    void Start () {
        laserBeam = GetComponentInChildren<ParticleSystem>();
        laserLine = GetComponentInChildren<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (beamActive && timer < journeyTime)
        {
            timer += Time.deltaTime;

            transform.rotation = Quaternion.Euler(transform.rotation.x, startAngleY + (360 *  timer/journeyTime), transform.rotation.z);
        } else
        {
            laserBeam.Stop();
            beamActive = false;
            timer = 0;
        }
	}

    public void StartLaser(Vector3 startLookAtPos)
    {
        transform.LookAt(startLookAtPos);

        laserBeam.Play();

        Invoke("StartRotation", 3f);

        startAngleY = transform.rotation.y;
        Debug.Log("Star angle Y " + startAngleY);
    }

    public void StartRotation()
    {
        timer = 0;
        beamActive = true;
    }
}
