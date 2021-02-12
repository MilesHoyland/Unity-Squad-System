using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum SquadState
{
    NULL = 0,
    FOLLOW = 1,
    FIGHT = 2
}

/// <summary>
/// A system to control the squad and give them their tasks.
/// Controls whether they should be fighting enemies or following the player.
/// It feeds the squad information based on the current player situation and world context.
/// Uses the World Intelligence class to take world context into account for Squad.
/// </summary>
public class SquadController : MonoBehaviour
{
    #region Sub-Systems
    //Manager
    [SerializeField] WorldIntelligence          m_worldIntelligence;
    [SerializeField] SquadManager               m_manager;
    [SerializeField] FormationManager           m_formationManager;
    #endregion

    #region Private Members
    //Useful Caches Input
    [SerializeField] GameObject                 m_player;
    [SerializeField] bool                       m_playerSeen = false;

    [SerializeField] SquadMemberAI[]            m_squad;
    [SerializeField] SquadState                 m_squadState = SquadState.NULL;


    //Squad Follow Data
    [SerializeField] List<GameObject>           m_validCrumbs;
    [SerializeField] GameObject                 m_formationAnchor;
    [SerializeField] GameObject[]               m_formations;
    [SerializeField] GameObject                 m_anchorCrumb = null;
    [SerializeField] float                      playerSpace = 2f;
    [SerializeField] float                      squadSpace = 10f;
    [SerializeField] Color                      playerSpaceColor = Color.magenta;
    [SerializeField] Color                      squadSpaceColor = Color.green;
    [SerializeField] bool                       squadClose = false;

    //Output
    [SerializeField] Vector3[]                  m_followPositions = null;
    
    //Crumb members
    GameObject[]                                crumbPool;
    Queue<GameObject>                           m_breadCrumbs = null;
    
    [SerializeField]                            GameObject m_lastCrumb;
    [SerializeField]                            float crumbSpacing = 4f;
    [SerializeField]                            int maxCrumbs = 10;
    [SerializeField]                            int crumbPoolIterator = 0;

    #endregion

    #region Public Members
    public GameObject                           Player { set { m_player = value; } }
    public SquadMemberAI[]                      Squad { set { m_squad = value; } }
    public SquadManager                         Manager {  set { m_manager = value; } }
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
        m_breadCrumbs = new Queue<GameObject>();

        m_anchorCrumb = m_player;

        if(!m_player)
            Debug.LogError("Player not properly linked.");
        if (!m_formationManager)
            Debug.LogError("Formation Manager not properly referenced.");
        if (!m_worldIntelligence)
            Debug.LogError("World intel not properly referenced.");

        m_squadState = SquadState.FOLLOW;

        //StartCoroutine(UpdateLoop());

    }
    /// <summary>
    ///     Sets up the Squad array and manager references for each member.
    ///     Initialises the squad follow positions and formation offsets to the ammount of squad members.
    ///     Inits the data to that of the currently selected formation.
    /// </summary>
    private void Awake()
    {
        m_squad = new SquadMemberAI[4];
        m_squad = m_manager.Squad.Squadies;

        if (m_squad != null)
        {
            foreach(var s in m_squad)
            {
                s.Manager = this.GetComponent<SquadController>();
            }

            // Initialise squad follow pos and formations arrays.
            m_formationAnchor = new GameObject();
            m_formationAnchor.name = "Follow Points Anchor";
            m_followPositions = new Vector3[m_squad.Length];
            m_formations = new GameObject[m_squad.Length];
            
            // Cache formation offsets.
            for(int i = 0; i < m_formations.Length; i++)
            {
                m_formations[i] = new GameObject();
                m_formations[i].name = "Follow Point " + i;
                m_formations[i].transform.position = m_formationManager.formationData.positions[i];
                m_squad[i].transform.Translate(m_formations[i].transform.localPosition);
                m_formations[i].transform.parent = m_formationAnchor.transform;
            }     
        }
        
    }

    /// <summary>
    /// Within this class a core loop of Update input, process & output is applied to the squad 
    /// </summary>
    void Update()
    {
        UpdateCacheData();
        ProcessData();
        ApplyOutput();
    }
    #endregion
    #region Utility Methods

    IEnumerator UpdateLoop()
    {
        //1) COLLATE UP TO DATE INPUT
        UpdateCacheData();

        //2) PROCESS DATA THEN EVALUATE RELEVANCE
        ProcessData();


        //3) UPDATE OUTPUTDATA & APPLY TO SQUAD
        ApplyOutput();

        yield return null;
    }

    /// <summary>
    ///     Updates the cache data for, the crumbs(the anchor crumb specifically), if the squad are within squad bounds
    ///     & if the player is seen.
    /// </summary>
    void UpdateCacheData()
    {
        UpdateCrumbs();

        //1) Calculate valid potential anchor crumbs
        List<GameObject> validCrumbs = new List<GameObject>();
        //get newest valid crumb
        foreach(var c in m_breadCrumbs)
        {
            float dist = Vector3.Distance(c.transform.position, m_player.transform.position);
            if (dist > playerSpace && dist < squadSpace)
            {
                validCrumbs.Add(c);
            }
        }
        m_validCrumbs = validCrumbs;

        if(validCrumbs.Count > 0)
        {
            m_anchorCrumb = validCrumbs[validCrumbs.Count - 1];
        }

        //2) Update if the squad are within the squad zone  
        squadClose = CheckSquadClose();

        //3) get world intel 
        m_playerSeen = m_worldIntelligence.PlayerSeen;
    }


    /// <summary>
    ///     A function to process the directions to be sent to the AI. It updates the follow positions List to reflect new psoitions
    ///     the squadies should latch on to around the anchor crumb.
    /// </summary>
    void ProcessData()
    {
        //0) Work out what state the squad should be in.
        //1) if player is not seen
        if (m_playerSeen)
        {
            m_squadState = SquadState.FIGHT;
        }
        if (!m_playerSeen)
        {
            if(!CheckSquadClose())
            {
                //0) Rotate the follow anchor to match the player
                Quaternion rotOffset = Quaternion.identity;
                rotOffset.eulerAngles.Set(0f,-90f,0f);

                m_formationAnchor.transform.rotation = m_player.transform.rotation;
                m_formationAnchor.transform.Rotate(0f, -90f, 0f);
                //1) Work out new follow positions.
                for (int i = 0; i < m_squad.Length; i++)
                {
                    Vector3 posOffset = m_anchorCrumb.transform.position + m_formations[i].transform.position;
                    m_followPositions[i] = posOffset;
                }
            }
        }


    }

    /// <summary>
    ///     A Function to apply all the goals and state changes to the AI.
    /// </summary>
    void ApplyOutput()
    {
        //1) Apply the most up-to-date state to all squadies. 
        ChangeAllMemberStates(m_squadState);
        
        //2) Apply new follow positions.
        if(m_squadState == SquadState.FOLLOW)
        {
            for(int i = 0; i < m_squad.Length; i++)
            {
                if(!m_squad[i].HasCommand)
                {
                    m_squad[i].FollowPosition = m_followPositions[i];
                    m_squad[i].Agent.SetDestination(m_followPositions[i]);
                }

            }
        }
    }

    //Checks if any of the squad members are out of the squad distance
    public bool CheckSquadClose()
    {
        bool withinDistanceFlag = true;
        foreach(SquadMemberAI s in m_squad)
        {
            if(Vector3.Distance(s.gameObject.transform.position, m_player.transform.position) > squadSpace + 5f)
            {
                withinDistanceFlag = false;  
            }
        }
       
        return withinDistanceFlag;
    }

    public EnemyAI RequestBestTarget(SquadMemberAI squadie)
    {
        List<EnemyAI> enemys = m_worldIntelligence.GetEnemies;
        EnemyAI closestEnemy = enemys[0];
        foreach(var e in enemys)
        {
            float dist = Vector3.Distance(squadie.transform.position, e.transform.position);
            float currClosestDist = Vector3.Distance(squadie.transform.position, closestEnemy.transform.position);

            if (dist < currClosestDist)
            {
                closestEnemy = e;
            }
        }

        return closestEnemy;
    }

    public Vector3 RequestFollowPoint(SquadMemberAI squadMember)
    {
        //1) get a formation offset position
        //the formation position is just an offset around an origin
        Vector3 formationOffset = Vector3.zero;

        //2) select a desireable crumb and apply formation offset to it
        Vector3 desireablePoint = m_validCrumbs[Random.Range(0, m_validCrumbs.Count)].transform.position;
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
        if(m_breadCrumbs.Count == 0)
        {
            crumbPool[crumbPoolIterator].transform.position = m_player.transform.position;
            m_breadCrumbs.Enqueue(crumbPool[crumbPoolIterator]);
            m_lastCrumb = crumbPool[crumbPoolIterator];
            crumbPoolIterator++;
        }

        //check if distance from last crumb is > than spacing var
        if (Vector3.Distance(m_player.transform.position, m_lastCrumb.transform.position) > crumbSpacing)
        {
            crumbPool[crumbPoolIterator].transform.position = m_player.transform.position;
            m_breadCrumbs.Enqueue(crumbPool[crumbPoolIterator]);
            m_lastCrumb = crumbPool[crumbPoolIterator];
            crumbPoolIterator++;
            if (m_breadCrumbs.Count >= maxCrumbs)
            {
                m_breadCrumbs.Dequeue();
            }
            if(crumbPoolIterator == maxCrumbs)
            {
                crumbPoolIterator = 0;
            }
        }
    }

    private void ChangeAllMemberStates(SquadState change)
    {
        foreach (var s in m_squad)
        {
            s.State = change;
        }
    }

    private void OnDrawGizmos()
    {
        //Crumbs editor visualisation
        if(m_breadCrumbs != null)
        {
            foreach(GameObject c in m_breadCrumbs)
            {
                float distFromPlayer = Vector3.Distance(c.transform.position, m_player.transform.position);
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
        Vector3 pos = m_player.transform.position + new Vector3(x, 0, y);
        Vector3 newPos = pos;
        Vector3 lastPos = pos;
        
        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = playerSpace * Mathf.Cos(theta);
            y = playerSpace * Mathf.Sin(theta);
            newPos = m_player.transform.position + new Vector3(x, 0, y);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }
        Gizmos.DrawLine(pos, lastPos);



        Gizmos.color = squadSpaceColor;

        theta = 0;
        x = squadSpace * Mathf.Cos(theta);
        y = squadSpace * Mathf.Sin(theta);
        pos = m_player.transform.position + new Vector3(x, 0, y);
        newPos = pos;
        lastPos = pos;
        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = squadSpace * Mathf.Cos(theta);
            y = squadSpace * Mathf.Sin(theta);
            newPos = m_player.transform.position + new Vector3(x, 0, y);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }
        Gizmos.DrawLine(pos, lastPos);


        foreach(var fp in m_followPositions)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(fp, 0.1f);
        }
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
