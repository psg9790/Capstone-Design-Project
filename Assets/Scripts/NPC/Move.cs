using UnityEngine;
using System.Collections; 
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    NavMeshAgent navAgent;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent> (); 
    }

    void Update()
    {
        Vector3 pos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray; 
            ray = Camera.main.ScreenPointToRay(pos); RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                navAgent.SetDestination(hitInfo.point); 
                
            } 
        }
        
    } 
} 