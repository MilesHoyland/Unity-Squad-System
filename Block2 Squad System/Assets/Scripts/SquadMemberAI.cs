using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(Collider))]

public class SquadMemberAI : MonoBehaviour
{
    //For now we'll just make a fsm for the different behaviour
    public NavMeshAgent nav_agent;
    public Collider collider;

    private void Start()
    {
        collider = this.GetComponent<Collider>();
        if(collider)
        {
            Debug.Log("Collider Initialized.");
        }
        if(!collider)
        {
            Debug.Log("ERROR: Did not initialize collider.");
        }
        nav_agent = this.GetComponent<NavMeshAgent>();
        if (!nav_agent)
            Debug.LogError("Navmesh Agent not found.");
        else
        {
            Debug.Log("Nav mesh agent set up");
        }
    }

    private void Update()
    {
        
    }



    public void GoToPoint(Vector3 point)
    {
        Debug.Log("Navmesh agent past status: " + nav_agent.pathStatus.ToString());
        Debug.Log("Navmesh agent GoToPoint x: " + point.x.ToString() + ", y: "
        + point.y.ToString() + ",z: " + point.z.ToString());
        nav_agent.SetDestination(point);
        Debug.Log("Navmesh agent past status: " + nav_agent.pathStatus.ToString());
    }

    public bool GoToLocation(Vector3 position, float sphere_radius)
    {
        if (nav_agent)
        {
            for (int i = 0; i < 20; i++)
            {
               
                Vector3 sample_pos = Random.insideUnitSphere * sphere_radius;
                sample_pos += position;

                sample_pos.y += sphere_radius; // add the minimum height to raise above any initial ocluded position
                RaycastHit hitInfo;
                Ray ray = new Ray(sample_pos, -Vector3.up);

                //if it hits
                if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
                {
                    sample_pos = hitInfo.point; // redefine sample pos to an environment surface location

                    
                    NavMeshHit hit;
                    if(NavMesh.SamplePosition(sample_pos, out hit, 1.0f, NavMesh.AllAreas))
                    {
                        sample_pos = hit.position; // redefine sample pos to be an appropriate navmesh position
                        nav_agent.SetDestination(sample_pos);
                        return true; // return true for safe position
                    }
                    else
                    {
                        i++;
                    }
                }
                else
                {
                    i++;
                }
            }
            return false; // returns false for bad position
        }
        else
        {
            return false; // returns true for bad position
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
