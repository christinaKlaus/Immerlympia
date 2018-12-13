using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyPropertyDrawer : PropertyDrawer {
	bool editable = false;
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
		//property.serializedObject.Update();

		ReadOnlyAttribute readOnlyAttribute = (ReadOnlyAttribute) attribute;

		EditorGUI.BeginProperty(position, label, property);
		
		if(readOnlyAttribute.activatable){
			Rect togglePosition = position;
			togglePosition.width = 16f;
			editable = EditorGUI.ToggleLeft(togglePosition, GUIContent.none, editable);
			position.x += 16f;
		}
		
		EditorGUI.BeginDisabledGroup(!editable);
		position = EditorGUI.PrefixLabel(position, label);
		position.x -= readOnlyAttribute.activatable ? 16f : 0f;
		EditorGUI.PropertyField(position, property, GUIContent.none);
		EditorGUI.EndDisabledGroup();
		EditorGUI.EndProperty();

		//property.serializedObject.ApplyModifiedProperties();
	}
}