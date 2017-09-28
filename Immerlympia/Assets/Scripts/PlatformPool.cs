using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformPool", menuName = "Pools/PlatformPool", order = 1)]

public class PlatformPool : ScriptableObject {

    public GameObject[] platformPool;


	public GameObject GetRandomPlatform() {

        int rndIndex = (int) (Random.value * platformPool.Length);

        return platformPool[rndIndex];
    }
}
