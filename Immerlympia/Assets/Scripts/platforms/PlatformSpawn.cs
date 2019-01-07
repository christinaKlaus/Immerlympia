using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawn : MonoBehaviour {

    public bool enablePlatformCycle = true;
    public PlatformPool pool;
    private PlatformScript[] platforms;

	// Use this for initialization
	void Start () {
        generatePool();
        newPlatform(0);
        newPlatform(1);
        newPlatform(2);
    }

    void generatePool(){
        platforms = new PlatformScript[pool.platformPool.Length];
        for(int i = 0; i < platforms.Length; i++) {
            platforms[i] = Instantiate(pool.platformPool[i], new Vector3(0, -1000, 0), Quaternion.identity, transform.transform).GetComponent<PlatformScript>();
            platforms[i].gameObject.SetActive(false);
        }
    }

    public void newPlatform(int position) {
        List<PlatformScript> possiblePlat = new List<PlatformScript>();
        foreach(PlatformScript p in platforms)
            if(!p.gameObject.activeSelf) possiblePlat.Add(p);

        PlatformScript newPlatform = possiblePlat[Random.Range(0,possiblePlat.Count)];
        newPlatform.currentPosition = position;
        newPlatform.gameObject.transform.rotation = Quaternion.Euler(0, 120 * position, 0);
        newPlatform.gameObject.transform.position = Vector3.zero;
        newPlatform.gameObject.SetActive(true);
        
    }
}
