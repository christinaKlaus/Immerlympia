using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Camera)), ExecuteInEditMode]
public class ReplacementShader : MonoBehaviour {

	public Material replacementShader;
	private Camera cam;
	[HideInInspector] public bool replacementShaderActive = false;

	public void Reset(){
		cam.ResetReplacementShader();
		replacementShaderActive = false;
	}

	public void Awake(){
		cam = GetComponent<Camera>();
	}

	public void Start(){
		cam.SetReplacementShader(replacementShader.shader, "");
	}

	public void ToggleReplacementShader(){
		if(cam == null || replacementShader == null) return;
		
		if(replacementShaderActive){
			cam.ResetReplacementShader();
			replacementShaderActive = false;
		} else {
			cam.SetReplacementShader(replacementShader.shader, "");
			replacementShaderActive = true;
		}

	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(ReplacementShader)), CanEditMultipleObjects]
public class ReplacementShaderEditor : Editor {
	public override void OnInspectorGUI(){
		ReplacementShader r = (ReplacementShader)serializedObject.targetObject;
		DrawDefaultInspector();
		if(GUILayout.Button(r.replacementShaderActive ? "Deactivate replacement shader" : "Activate replacement shader")){
			if(r.replacementShaderActive){
				r.Reset();
			} else {
				r.ToggleReplacementShader();
			}
		}
	}
}
#endif