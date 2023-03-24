using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

//Intended to be assigned to MainCamera. (Make sure it's tagged "MainCamera").

public class PointAndClickManager : MonoBehaviour
{
    public GameObject target;           //Stores Player object
        
    public float cameraDistance = 8f;   //Determines how close game camera should be to the Player (zoom)

    private NavMeshAgent agent;         //NavMesh agent component of the Player object
    private Vector3 offset;             //Camera offset to the player object
    private Ray ray;                    //Used in raycasting


    void Awake()
    {
        agent = target.GetComponent<NavMeshAgent>();                                    //Assign NavMesh component

        transform.position = target.transform.position + new Vector3(1f, 1.5f, -1f);    //Set camera relative to the Player

        offset = transform.position - target.transform.position;                        //Get camera offset

        offset *= cameraDistance / offset.magnitude;                                    //Apply camera distance to the offcet

        transform.position = target.transform.position + offset;                        //Apply changed offcet back to the camera

        transform.LookAt(target.transform.position);                                    //Look at Player's position
    }


    void Update()
    {
        
        RaycastHit hit;                                                     //Store an object hit by a ray

            if (Input.GetMouseButton(0))                                    //If Left Mouse button is held...
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //...create a ray from mouse position.

            if (Physics.Raycast(ray, out hit, 20))                          //Cast the ray. If it his anything...
                {
                    agent.SetDestination(hit.point);                        //...tell NavMesh on the Player object to set new destination
                }
            }

        transform.position = Vector3.Lerp(transform.position, target.transform.position + (offset * cameraDistance / offset.magnitude), Time.deltaTime * 10);   //Make camera follow the Player. Allows realtime zoom.
        
    }

   
}

