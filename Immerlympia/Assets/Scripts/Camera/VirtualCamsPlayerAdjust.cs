using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VirtualCamsPlayerAdjust : MonoBehaviour {

	public Transform[] followTargets;
	public Transform[] lookAtTargets;

	public void SetTargets(){
		if(followTargets.Length != 4 || lookAtTargets.Length != 4 || transform.childCount != 4){
			throw new Exception("Virtual camera adjustment failed (targets and child count out of sync)");
		}
		for(int i = 0; i < transform.childCount; i++){
			Cinemachine.CinemachineVirtualCamera vCam = transform.GetChild(i).GetComponent<Cinemachine.CinemachineVirtualCamera>();
			vCam.Follow = followTargets[i];
			vCam.LookAt = lookAtTargets[i];
		}
	}


}

#if UNITY_EDITOR
[CustomEditor(typeof(VirtualCamsPlayerAdjust))]
public class VirtualCamsPlayerAdjustEditor : Editor {
	public override void OnInspectorGUI(){
		VirtualCamsPlayerAdjust vcpa = serializedObject.targetObject as VirtualCamsPlayerAdjust;
		DrawDefaultInspector();
		if(GUILayout.Button("Set targets")){
			vcpa.SetTargets();
		}
	}
}

#endif