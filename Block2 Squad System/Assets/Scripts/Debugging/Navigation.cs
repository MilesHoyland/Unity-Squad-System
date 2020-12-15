using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Navigation : MonoBehaviour
{
    private bool debug = true;

    [SerializeField]
    private NavMeshAgent agentToDebug;
    private LineRenderer lineRenderer;

    //eventually will be used with all agents we want to see debug information for.
    private List<AIRoute> routesToDebug;

    struct AIRoute
    {
        private NavMeshAgent agent;

        private GameObject endLocator;
        Vector3 startPosition;
        Vector3 desiredPosition;
        bool showRoute;
        bool persistPositions;
        

        AIRoute(NavMeshAgent agent, GameObject destinationPrefab)
        {
            this.agent = agent;
            endLocator = destinationPrefab;
            startPosition = Vector3.zero;
            desiredPosition = Vector3.zero;
            showRoute = false;
            persistPositions = true;
            
            if(agent.hasPath)
            {            
                desiredPosition = agent.path.corners[agent.path.corners.Length];
            }
        }
        
    }


    void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();

    }

    void Update()
    {
        

    }


    // Get random point on navmesh
    public Vector3 GetPointInSphere(Vector3 center, float radius, int maxIterations)
    {
        for(int i = 0; i < maxIterations; i++)
        {
            // Get Random Hit iside a sphere with radius
            Vector3 randomPos = Random.insideUnitSphere * radius + center;
            NavMeshHit hit;

            // from randomPos find a nearest point on NavMesh surface in range of maxDistance
            if(NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }
            else
            {
                i++;
            }
        }
        Debug.Log("No Position found return z.");
        return Vector3.zero;
    }

}
