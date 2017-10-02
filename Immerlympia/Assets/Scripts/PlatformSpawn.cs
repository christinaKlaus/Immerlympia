using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawn : MonoBehaviour {

    public PlatformPool pool;
    private GameObject[] platforms;

	// Use this for initialization
	void Start () {
        generatePool();
        newPlatform(0);
        newPlatform(1);
        newPlatform(2);
    }

    void generatePool(){
        platforms = new GameObject[pool.platformPool.Length];
        for(int i = 0; i < platforms.Length; i++) {
            platforms[i] = Instantiate(pool.platformPool[i], new Vector3(0, -1000, 0), Quaternion.identity, transform.transform);
            platforms[i].SetActive(false);    
        }
    }

    public void newPlatform(int position) {
        List<GameObject> possiblePlat = new List<GameObject>();
        foreach(GameObject g in platforms)
            if(!g.activeSelf) possiblePlat.Add(g);

        GameObject newPlatform = possiblePlat[Random.Range(0,possiblePlat.Count)];
        newPlatform.transform.rotation = Quaternion.Euler(0, 120 * position, 0);
        newPlatform.transform.position = Vector3.zero;
        newPlatform.SetActive(true);
        
    }
}
