 using UnityEngine;
 using UnityEditor;     
 public class EditorShortCuts : ScriptableObject
 {    
     [MenuItem("GameObject/Deselect all %#D")]
     static void DeselectAll()
     {
         Selection.objects = new Object[0];
     }

     [MenuItem("Tools/Remove Navigation Static Flag")]
     static void RemoveNavFlag(){
        foreach(GameObject go in Selection.gameObjects){
            foreach(Transform t in go.GetComponentsInChildren<Transform>()){
                GameObjectUtility.SetStaticEditorFlags(t.gameObject, GameObjectUtility.GetStaticEditorFlags(t.gameObject) & ~(StaticEditorFlags.NavigationStatic));
            }
        }
     }
}
