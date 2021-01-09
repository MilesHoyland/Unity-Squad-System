using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A system to control the squad and give them their tasks.
/// Controls whether they should be fighting enemies or following the player.
/// It feeds the squad information based on the current player situation and world context.
/// Uses the World Intelligence class to take world context into account for Squad.
/// </summary>
public class SquadDirector : MonoBehaviour
{
    #region Private Members
    [SerializeField] GameObject player = null;

    //Crumb members
    GameObject[] crumbPool;
    [SerializeField]  GameObject lastCrumb;
    [SerializeField]  int crumbPoolIterator = 0;

    Queue<GameObject> breadCrumbs = null;
    [SerializeField] float crumbSpacing = 4f;
    [SerializeField] int maxCrumbs = 10;

    //Relevant squad follow crumbs
    private List<GameObject> desireableCrumbs;

    //Squad follow position ranges
    [SerializeField] float playerSpace = 1f;
    [SerializeField] float squadSpace = 4f;
    [SerializeField] private Color playerSpaceColor = Color.magenta;
    [SerializeField] Color squadSpaceColor = Color.green;
    #endregion

    #region Public Members
    #endregion

    #region Main Methods
    void Start()
    {
        crumbPool = new GameObject[maxCrumbs];
        for(int i = 0; i < maxCrumbs; i++)
        {
            GameObject go = new GameObject();
            go.name = "crumbPoolPiece" + i.ToString();
            crumbPool[i] = go;
        }
        breadCrumbs = new Queue<GameObject>();

        if(!player)
            Debug.LogError("Player not properly linked.");
        
    }

    void Update()
    {
        UpdateCrumbs();

    }
    #endregion
    #region Utility Methods

    public Vector3 RequestFollowPoint(SquadMemberAI squadMember)
    {
        //1) get a formation offset position
        //the formation position is just an offset around an origin
        Vector3 formationOffset = Vector3.zero;

        //2) select a desireable crumb and apply formation offset to it
        Vector3 desireablePoint = desireableCrumbs[Random.Range(0,desireableCrumbs.Count)].transform.position;
        desireablePoint += formationOffset;

        //3) ensure point returned is a navmeshable point
        desireablePoint = GetPointInSphere(desireablePoint, 0.3f, 10);
        if(desireablePoint == Vector3.zero)
        {
            Debug.LogWarning("A desireable follow point for " + squadMember.gameObject.name + " could not be found, thus was set to Vector3.zero");
        }

        return desireablePoint;
    }


    void UpdateCrumbs()
    {
        //check to see if first crumb needs placing & place
        if(breadCrumbs.Count == 0)
        {
            crumbPool[crumbPoolIterator].transform.position = player.transform.position;
            breadCrumbs.Enqueue(crumbPool[crumbPoolIterator]);
            lastCrumb = crumbPool[crumbPoolIterator];
            crumbPoolIterator++;
        }

        //check if distance from last crumb is > than spacing var
        if (Vector3.Distance(player.transform.position, lastCrumb.transform.position) > crumbSpacing)
        {
            crumbPool[crumbPoolIterator].transform.position = player.transform.position;
            breadCrumbs.Enqueue(crumbPool[crumbPoolIterator]);
            lastCrumb = crumbPool[crumbPoolIterator];
            crumbPoolIterator++;
            if (breadCrumbs.Count >= maxCrumbs)
            {
                breadCrumbs.Dequeue();
            }
            if(crumbPoolIterator == maxCrumbs)
            {
                crumbPoolIterator = 0;
            }
        }
    }

    

    private void OnDrawGizmos()
    {
        //Crumbs editor visualisation
        if(breadCrumbs != null)
        {
            foreach(GameObject c in breadCrumbs)
            {
                float distFromPlayer = Vector3.Distance(c.transform.position, player.transform.position);
                if (distFromPlayer < squadSpace && distFromPlayer > playerSpace)
                {
                    //desireable crumbs
                    Gizmos.color = Color.green;
                }
                else
                {
                    //rest of the shop
                    Gizmos.color = Color.white;
                }
                Gizmos.DrawWireSphere(c.transform.position, 0.2f);
            }
        }

        //Squadzones
        Gizmos.color = playerSpaceColor;

        float theta = 0;
        float x = playerSpace * Mathf.Cos(theta);
        float y = playerSpace * Mathf.Sin(theta);
        Vector3 pos = player.transform.position + new Vector3(x, 0, y);
        Vector3 newPos = pos;
        Vector3 lastPos = pos;
        
        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = playerSpace * Mathf.Cos(theta);
            y = playerSpace * Mathf.Sin(theta);
            newPos = player.transform.position + new Vector3(x, 0, y);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }
        Gizmos.DrawLine(pos, lastPos);



        Gizmos.color = squadSpaceColor;

        theta = 0;
        x = squadSpace * Mathf.Cos(theta);
        y = squadSpace * Mathf.Sin(theta);
        pos = player.transform.position + new Vector3(x, 0, y);
        newPos = pos;
        lastPos = pos;
        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = squadSpace * Mathf.Cos(theta);
            y = squadSpace * Mathf.Sin(theta);
            newPos = player.transform.position + new Vector3(x, 0, y);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }
        Gizmos.DrawLine(pos, lastPos);
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
    #endregion
}
