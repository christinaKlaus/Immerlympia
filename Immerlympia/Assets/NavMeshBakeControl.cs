using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshBakeControl : MonoBehaviour
{
 
    [ReadOnly(false)] public bool isBaking = false;
    NavMeshSurface navMesh;

    void Awake(){
        navMesh = GetComponent<NavMeshSurface>();
    }

    public void UpdateNavmeshControl(){
        // navMesh.UpdateNavMesh(navMesh.navMeshData);
        Debug.Log("Update Navmesh Control called", this);
        if(!isBaking){
            Debug.Log("Update Navmesh Control started baking", this);
            isBaking = true;
            AsyncOperation baking = navMesh.UpdateNavMesh(navMesh.navMeshData);
            baking.completed += (op) => {
                isBaking = op.isDone;
            };
        }
    }


#if UNITY_EDITOR

    [MenuItem("Tools/Bake Selected Navmesh")]
    public static void BakeSelectedNavmesh(){
        NavMeshSurface navmeshToBake = Selection.activeTransform.GetComponent<NavMeshSurface>();
        if(navmeshToBake == null) return;
        else {
            // navmeshToBake.UpdateNavMesh(navmeshToBake.navMeshData);
            navmeshToBake.BuildNavMesh();
        }
    }

#endif


}
