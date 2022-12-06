using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentTesting : MonoBehaviour
{
    static NavMeshAgent agent;
    void Awake()
    {
        if(agent == null)
            agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)){
                agent.SetDestination(hit.point);
            }
        }
    }

    public static void SetDestination(Vector3 point){
        if(agent){
            agent.SetDestination(point);
        }
    }
}
