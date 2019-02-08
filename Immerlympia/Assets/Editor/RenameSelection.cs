using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RenameSelection : EditorWindow
{

    static string nameBase = "", prefix = "", suffix = "";
    static bool numbered = false, numberInFront = true;
    static int baseNumber = 0, step = 1;


    [MenuItem ("Window/Rename Selection")]
    public static void ShowWindow () {
        EditorWindow.GetWindow(typeof(RenameSelection));
    }

    public void OnGUI(){

        nameBase = EditorGUILayout.TextField("name base", nameBase);
        
        prefix = EditorGUILayout.TextField("prefix", prefix);
        suffix = EditorGUILayout.TextField("suffix", suffix);

        numbered = EditorGUILayout.BeginToggleGroup("numbered", numbered);
        numberInFront = EditorGUILayout.Toggle("number in front", numberInFront);
        baseNumber = EditorGUILayout.IntField("base number", baseNumber);
        step = EditorGUILayout.IntField("step", step);
        EditorGUILayout.EndToggleGroup();

        EditorGUI.BeginDisabledGroup(nameBase.Length == 0 && prefix.Length == 0 && suffix.Length == 0);
        if(GUILayout.Button("rename selection")){
            RenameSelectedGameObjects();
        }
        EditorGUI.EndDisabledGroup();

    }

    public void RenameSelectedGameObjects(){
        GameObject[] objectsToRename = Selection.gameObjects;
        System.Array.Sort(objectsToRename, new UnityTransformSort());

        int maxDigits = (baseNumber + step * (objectsToRename.Length - 1)).ToString().Length;
        string format = "";
        for(; maxDigits > 0; maxDigits--)
            format += "0";

        int i = 0;

        Undo.RecordObjects(objectsToRename, "selection renamed");
        foreach(GameObject go in objectsToRename){
            go.name =   (numbered && numberInFront ? (baseNumber + (step * i++)).ToString(format) : "") + 
                        prefix + (nameBase.Length > 0 ? nameBase : go.name) + suffix + 
                        (numbered && !numberInFront ? (baseNumber + (step * i++)).ToString(format) : "");
        }
    }
}

public class UnityTransformSort: System.Collections.Generic.IComparer<GameObject>
 {
    public int Compare(GameObject lhs, GameObject rhs)
     {
         if (lhs == rhs) return 0;
         if (lhs == null) return -1;
         if (rhs == null) return 1;
         return (lhs.transform.GetSiblingIndex() > rhs.transform.GetSiblingIndex()) ? 1 : -1;
     }
 }
