using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class CameraStackManager : MonoBehaviour
{

    public LabeledCamera[] labeledCameras = new LabeledCamera[4];


}

[System.Serializable]
public class LabeledCamera {
    public string cameraName;
    public Camera camera;
}

#if UNITY_EDITOR

[CustomEditor(typeof(CameraStackManager))]
public class CameraStackManagerEditor : Editor {
    public override void OnInspectorGUI(){
        Rect signPostRect = EditorGUILayout.GetControlRect();
        EditorGUI.DrawRect(signPostRect, Color.yellow);
        EditorGUI.LabelField(signPostRect, "Attention: SO FAR UNUSED SCRIPT", EditorStyles.centeredGreyMiniLabel);
        DrawDefaultInspector();
    }
}

[CustomPropertyDrawer(typeof(LabeledCamera))]
public class LabeledCameraDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){

        label = EditorGUI.BeginProperty(position, label, property);

        position.width *= 0.5f;
        
        property.FindPropertyRelative("cameraName").stringValue = EditorGUI.DelayedTextField(position, property.FindPropertyRelative("cameraName").stringValue);

        position.x += position.width;

        EditorGUI.ObjectField(position, property.FindPropertyRelative("camera"), GUIContent.none);

        EditorGUI.EndProperty();

        property.serializedObject.ApplyModifiedProperties();
    }

    // public override float GetProp
}

#endif