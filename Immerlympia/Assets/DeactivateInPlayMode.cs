using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeactivateInPlayMode : MonoBehaviour
{
    [SerializeField]
    bool activateOnPlay = false;
    void Awake(){
        if(Application.isPlaying){
            gameObject.SetActive(activateOnPlay);
        }
    }
}
