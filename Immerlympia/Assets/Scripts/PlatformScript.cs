using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

    
    public Vector2 lifetimeRange;
    public float fallSpeed;
    public float warning;
    public float shakiness;
    public float appearTime;

    private float timer = 8;
    private float duration;

    private Vector3[] basePos;

    // Use this for initialization
    void OnEnable () {
        timer = Random.Range(lifetimeRange.x, lifetimeRange.y);
        duration = timer;

        Update();
    }

    // Update is called once per frame
    void Update () {
        timer -= Time.deltaTime; // zählt Sekundenweise	
        //Debug.Log(timer);

        if (timer < warning) {
            if(basePos == null) {
                basePos = new Vector3[transform.childCount];
                for (int i = 0; i < transform.childCount; i++)
                    basePos[i] = transform.GetChild(i).localPosition;
            }

            Vector3 offset = new Vector3(Random.Range(-shakiness, shakiness), Random.Range(-shakiness, shakiness), Random.Range(-shakiness, shakiness));

            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).localPosition = basePos[i] + offset;
        }

        if(timer > 0) {
            transform.position = -Vector3.up * Mathf.Min(timer * -1 + duration - appearTime, 0) * Mathf.Min(timer * -1 + duration - appearTime, 0) * fallSpeed;
        } else {
            transform.position = -Vector3.up * Mathf.Min(timer, 0) * Mathf.Min(timer, 0) * fallSpeed;
        }

        if (timer < -appearTime) {
            Remove();
        }

        
    }

    void Remove () {
        transform.GetComponentInParent<PlatformSpawn>().newPlatform((int)transform.rotation.eulerAngles.y / 120);
        transform.position = new Vector3(0, -1000, 0);
        gameObject.SetActive(false);
    }
}
