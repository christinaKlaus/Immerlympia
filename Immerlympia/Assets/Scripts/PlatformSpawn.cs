using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawn : MonoBehaviour {

    public PlatformPool pool;

	// Use this for initialization
	void Start () {
        newPlatform(0);
        newPlatform(1);
        newPlatform(2);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void newPlatform(int position) {
        GameObject newPlatform = Instantiate(pool.GetRandomPlatform());
        newPlatform.transform.rotation = Quaternion.Euler(0, 120 * position, 0);
        newPlatform.transform.position = Vector3.zero;

        newPlatform.transform.SetParent(transform);
    }
}
