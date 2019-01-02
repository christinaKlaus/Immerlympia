using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using System.Reflection;
#endif

public class UIConnector : MonoBehaviour {
	public List<VerticalConnections> connections;

}

[System.Serializable]
public class VerticalConnections {
	public Selectable[] connectionItems = new Selectable[2];

	public void MakeConnections(bool vertical){
		Debug.Log("Making connections ... ");
		if(connectionItems.Length >= 2){
			for(int i = 0; i < connectionItems.Length; i++){
				Debug.Log("... for " + connectionItems[i].gameObject.name);
				Navigation newNav = new Navigation();
				newNav.mode = Navigation.Mode.Explicit;
				int before = (i + (connectionItems.Length - 1)) % connectionItems.Length;
				int next = (i + 1) % connectionItems.Length;
				//Debug.Log("before: " + before + " next: " + next);
				if(vertical){
					newNav.selectOnUp = connectionItems[before];
					newNav.selectOnDown = connectionItems[next];
				} else {
					newNav.selectOnLeft = connectionItems[before];
					newNav.selectOnRight = connectionItems[next];
				}
				connectionItems[i].navigation = newNav;
			}
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(UIConnector))]
public class UIConnectVerticalEditor : Editor {
	
	UIConnector uIConnectVertical;
	public void OnEnable(){
		uIConnectVertical = serializedObject.targetObject as UIConnector;
	}

	/*
	public override void OnInspectorGUI(){
		SerializedProperty vcArray = serializedObject.FindProperty("connections");

		for(int i = 0; i < vcArray.arraySize; i++){
			if(GUILayout.Button("Connect", EditorStyles.miniButton)){
				uIConnectVertical.connections[i].MakeConnections();
			}
			EditorGUILayout.PropertyField(vcArray.GetArrayElementAtIndex(i));
		}
	}
	*/
}

[CustomPropertyDrawer(typeof(VerticalConnections))]
public class VerticalConnectionsDrawer : PropertyDrawer {
	
	SerializedProperty connectionsArray;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
		EditorGUI.BeginProperty(position, label, property);
		property.serializedObject.Update();

		GUI.skin.button.wordWrap = true;
		EditorStyles.miniButtonLeft.wordWrap = true;
		EditorStyles.miniButtonRight.wordWrap = true;

		Rect buttonRect = new Rect();
		if(property.isExpanded){
			buttonRect = new Rect(position.x, position.y, position.width * 0.25f, EditorGUI.GetPropertyHeight(connectionsArray, true) * 0.5f);
		} else {
			buttonRect = new Rect(position.x, position.y, position.width * 0.25f * 0.5f, EditorGUI.GetPropertyHeight(connectionsArray, true));
		}
		
		Rect arrayRect = new Rect(position.x + (position.width * 0.25f), position.y, position.width - (position.width * 0.25f), position.height);

		if(GUI.Button(buttonRect, "|")){
			VerticalConnections thisVertCons = GetActualObject<VerticalConnections>(fieldInfo, property);
			Debug.Log(thisVertCons.connectionItems.Length);
			thisVertCons.MakeConnections(true);
		}

		if(property.isExpanded)
			buttonRect.y += buttonRect.height;
		else
			buttonRect.x += buttonRect.width;

		if(GUI.Button(buttonRect, "--")){
			VerticalConnections thisVertCons = GetActualObject<VerticalConnections>(fieldInfo, property);
			Debug.Log(thisVertCons.connectionItems.Length);
			thisVertCons.MakeConnections(false);
		}
		EditorGUI.PropertyField(arrayRect, connectionsArray, true);
		position.height *= connectionsArray.isExpanded ? connectionsArray.arraySize : 1;

		/*
		for(int i = 0; i < connectionsArray.arraySize; i++){
			EditorGUI.PropertyField(position, connectionsArray.GetArrayElementAtIndex(i));
			position.y += EditorGUIUtility.singleLineHeight;
		}
		*/

		/*
		SerializedProperty selectablesArray = property.FindPropertyRelative("connections");
		position.height = EditorGUIUtility.singleLineHeight * (selectablesArray.arraySize + 1);

		Rect buttonRect = new Rect(position.x, position.y, position.width * 0.25f, EditorGUIUtility.singleLineHeight);
		Rect arraySizeRect = new Rect(position.x + buttonRect.width, position.y, position.width - buttonRect.width, EditorGUIUtility.singleLineHeight);

		if(GUI.Button(buttonRect, "Connect")){
			VerticalConnections thisVertCons = GetActualObject<VerticalConnections>(fieldInfo, property);
			thisVertCons.MakeConnections();
		}
		
		selectablesArray.arraySize = EditorGUI.IntField(arraySizeRect, selectablesArray.arraySize);
		arraySize = selectablesArray.arraySize;

		int originalIndent = EditorGUI.indentLevel;
		EditorGUI.indentLevel += 10;

		Rect arrayRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
		EditorGUILayout.PropertyField(selectablesArray);
		*/
		/*
		int i = 0;
		foreach(SerializedProperty p in selectablesArray){
			EditorGUI.indentLevel -= 2;
			EditorGUI.LabelField(arrayRect, "" + i++);
			EditorGUI.indentLevel += 2;
			EditorGUI.PropertyField(arrayRect, p, GUIContent.none);
			arrayRect.y += EditorGUIUtility.singleLineHeight;
		}
		*/

		//EditorGUI.indentLevel = originalIndent;
		property.serializedObject.ApplyModifiedProperties();
		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
		connectionsArray = property.FindPropertyRelative("connectionItems");
		return EditorGUI.GetPropertyHeight(connectionsArray, true);
	}


	public T GetActualObject<T>(FieldInfo fieldInfo, SerializedProperty property) where T : class {
		var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
		if(obj == null) return null;

		T actualObject = null;
		if(obj.GetType().IsArray){
			var index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
			actualObject = ((T[])obj)[index];
		} else if(obj.GetType() == typeof(List<T>)){
			var index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
			actualObject = (obj as List<T>)[index];
		} else {
			actualObject = obj as T;
		}
		return actualObject;
	}

}
#endif
