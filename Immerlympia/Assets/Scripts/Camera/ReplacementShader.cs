using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Camera)), ExecuteInEditMode]
public class ReplacementShader : MonoBehaviour {

	public enum Replace {
		Shader,
		Material,
		None
	}

	[SerializeField] bool useMaterialShader = true;
	[SerializeField] Shader replacementShader;
	[SerializeField] Material replacementMaterial;
	[SerializeField] string replacementKeyword = "XRay";
	private Camera cam;
	[ReadOnly(false)] public bool replacementShaderActive = false, replacementMaterialActive = false;

	public void Awake(){
		cam = GetComponent<Camera>();
	}

	public void Start(){
		ToggleReplacementShader(useMaterialShader ? Replace.Material : Replace.Shader, false);
	}

	/// <summary>Toggles the replacement shader on (true), off (false) or to inverse state (null)</summary>
	public void ToggleReplacementShader(Replace replace, bool checkActive){
		if(replacementShader == null && replacementMaterial == null) return;
		
		switch(replace){
			case Replace.Material:
				if((checkActive && replacementMaterialActive) || replacementMaterial == null) 
					ToggleReplacementShader(Replace.None, false);
				else {
					cam.SetReplacementShader(replacementMaterial.shader, replacementKeyword);
					replacementMaterialActive = true;
					replacementShaderActive = false;
					// Debug.Log("Using material replacement", this);
				}
				break;
			case Replace.Shader:
				if((checkActive && replacementShaderActive) || replacementShader == null) 
					ToggleReplacementShader(Replace.None, false);
				else {
					cam.SetReplacementShader(replacementShader, replacementKeyword);
					replacementShaderActive = true;
					replacementMaterialActive = false;
					// Debug.Log("Using shader replacement", this);
				}
				break;
			case Replace.None:
				cam.ResetReplacementShader();
				replacementMaterialActive = replacementShaderActive = false;
				break;
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(ReplacementShader)), CanEditMultipleObjects]
public class ReplacementShaderEditor : Editor {
	public override void OnInspectorGUI(){
		ReplacementShader r = (ReplacementShader)serializedObject.targetObject;
		DrawDefaultInspector();
		GUILayout.BeginHorizontal();
		if(GUILayout.Button(r.replacementShaderActive ? "deactivate shader" : "activate shader")){
			if(Selection.gameObjects.Length > 1){
				foreach(GameObject go in Selection.gameObjects){
					go.GetComponent<ReplacementShader>().ToggleReplacementShader(ReplacementShader.Replace.Shader, true);
				}
			} else {
				r.ToggleReplacementShader(ReplacementShader.Replace.Shader, true);
			}
		}
		if(GUILayout.Button(r.replacementMaterialActive ? "deactive material" : "activate material")){
			if(Selection.gameObjects.Length > 1){
				foreach(GameObject go in Selection.gameObjects){
					go.GetComponent<ReplacementShader>().ToggleReplacementShader(ReplacementShader.Replace.Material, true);
				}
			} else {
				r.ToggleReplacementShader(ReplacementShader.Replace.Material, true);
			}
		}
		GUILayout.EndHorizontal();
	}
}
#endif