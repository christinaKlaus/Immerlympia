using UnityEngine;
using UnityEditor;
using System.Collections;

public class LockPreviewEditor : EditorWindow {
	[SerializeField] Object[] objectsToPreview;
	int squareSize = 256;
	float spacing = 16f;

    [MenuItem ("Window/Lock Previews")]
    public static void ShowWindow () {
        EditorWindow.GetWindow(typeof(LockPreviewEditor));
    }

	public void OnEnable(){
		LoadEditor();
	}

	public void OnGUI(){
		autoRepaintOnSceneChange = true;
		ScriptableObject target = this;
		SerializedObject so = new SerializedObject(target);
		SerializedProperty previewArray = so.FindProperty("objectsToPreview");

		squareSize = EditorGUILayout.IntField("preview size", squareSize);

		GUILayout.Label("Drag scene/project objects here to create a preview window for them", EditorStyles.wordWrappedLabel);
		EditorGUILayout.PropertyField(previewArray, true);

		so.ApplyModifiedProperties();

		Rect textureRect = EditorGUILayout.GetControlRect();
		textureRect.x = spacing;
		textureRect.width = textureRect.height = squareSize;

		if(objectsToPreview != null){
			for(int i = 0; i < objectsToPreview.Length; i++){
				if(objectsToPreview[i].GetType() == typeof(RenderTexture)){
					EditorGUI.DrawPreviewTexture(textureRect, (RenderTexture) objectsToPreview[i]);
					textureRect.x += spacing + textureRect.width;
				}
			}
		}
	}

	public void OnDisable(){
		SaveEditor();
	}

	public void LoadEditor(){
		LockPreviewEditorPreset lpe = (LockPreviewEditorPreset) AssetDatabase.LoadAssetAtPath("Assets/Editor/LockPreviewEditor.asset", typeof(LockPreviewEditorPreset));
		objectsToPreview = lpe.previewObjects;
	}

	public void SaveEditor(){
		LockPreviewEditorPreset lpe = ScriptableObject.CreateInstance<LockPreviewEditorPreset>();
		lpe.previewObjects = objectsToPreview;
		AssetDatabase.CreateAsset(lpe , "Assets/Editor/LockPreviewEditor.asset");
	}
}

