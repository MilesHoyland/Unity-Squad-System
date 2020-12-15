using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquadController : MonoBehaviour
{
    public GameObject targetIndicator;
    public float commandRadius = 5f;
    public Squad squad;
    public Navigation navigation;

    public GameObject[] smates;
    
    void Start()
    {   
        //TODO change to get from squad system script
        if(squad)
        {
            smates = new GameObject[squad.squad.Length];
            for(int i = 0; i < squad.squad.Length; i++)
            {
                smates[i] = squad.squad[i].gameObject;
                Debug.Log("Squad mate: " + squad.squad[i].name + " was added to the squad controller.");
            }
        }
        if (!squad)
        {
            Debug.LogError("Squad instance not properly linked.");
        }
        if(!navigation)
        {
            Debug.LogError("Navigation not properly referenced.");
        }


        if (!targetIndicator)
        {
            Debug.LogError("Command Indicator not set up properly."); 
        }
        else
        {

        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Instantiate(targetIndicator, hitInfo.point, targetIndicator.transform.rotation);
                foreach (SquadMemberAI sm in squad.squad)
                {
                    //TODO implement navmesh control
                    if (sm.nav_agent)
                    {
                        Vector3 point = navigation.GetPointInSphere(hitInfo.point, 5f, 10);
                        sm.nav_agent.SetDestination(point);
                    }

                }
                //memberAI.GoToLocation(hitInfo.point, commandRadius);
                // memberAI.GoToPoint(hitInfo.point);
            }
                

        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Execute Squad Command: Go to Location");

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Execute Squad Command: Regroup");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Execute Squad Command: Hold position");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Execute Squad Command:  Aggressive Push");
        }
        
        if(Input.GetKeyDown(KeyCode.Q))
        {

        }
        //(1)Prefabs of modular areas with a box collider.
        // A context specific script called ContextCommand executes on an on trigger enter of the collider, 
        // it grabs access to the SquadController from the SquadMemberAI script and puts the 
        // squad mate ai into the State.ContextCommand and the state is only changed when
        // another group SquadCommand is called.


        //Perform context specific event, that takes the 
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }

    private void Regroup()
    {
        foreach(GameObject sm in smates)
        {
            //Change state in SquadMemberAI to Regroup
            SquadMemberAI memberAI = sm.GetComponent<SquadMemberAI>(); 
        }
    }

    private void GoToLocation()
    {
    }


}

