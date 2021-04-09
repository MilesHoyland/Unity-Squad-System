using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldIntelligence : MonoBehaviour
{
    public static WorldIntelligence instance = null;

    #region Sub-Systems
    [SerializeField] SystemManager m_systemManager;
    [SerializeField] SquadManager m_squadManager;
    #endregion

    #region Members
    //Player Data
    [SerializeField] GameObject m_player;
    [SerializeField] bool isPlayerSeen = false;
    [SerializeField] bool inCombat = false;

    //Squadies
    [SerializeField] SquadMemberAI[] m_squadieAI;

    //Enemys
    [SerializeField] List<EnemyAI> m_enemys;
    [SerializeField] List<EnemyAI> m_aliveEnemys;

    //Points Of Interest & customEvents
    //List<POI> m_poi

    //Items
    //List<Items> m_items;

    #endregion

    #region Public Members
    public bool PlayerSeen { get { return isPlayerSeen ; } set { isPlayerSeen = value ; } }
    public bool InCombat { get { return inCombat; } set { inCombat = value; } }
    public Transform Player { get { return m_player.transform; } }
    #endregion

    #region Main Methods
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!m_player)
            Debug.LogError("Player not referenced.");
        if (!m_systemManager)
            Debug.LogError("System manager not linked.");
        if (!m_squadManager)
            Debug.LogError("Squad not linked.");


        GetAllEnemies();
        GetAllSquadies();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inCombat = true;
            isPlayerSeen = true;
            Debug.Log("In Combat = true");
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            inCombat = false;
            isPlayerSeen = false;
            Debug.Log("In Combat = false");
        }
    }
    #endregion

    #region Utility Methods
    /// <summary>
    /// This sets up the enemys data. If an enemy dies it must remove itself from this list.
    /// </summary>
    void GetAllEnemies()
    {
        EnemyAI[] enemyArray = FindObjectsOfType<EnemyAI>();
        foreach(EnemyAI e in enemyArray)
        {
            m_enemys.Add(e);
            if(!e.IsDead)
            {
                m_aliveEnemys.Add(e);
            }
        }
    }

    void GetAllSquadies()
    {
        m_squadieAI = FindObjectsOfType<SquadMemberAI>();
    }

    public void EnemyDied(EnemyAI enemy)
    {
        m_aliveEnemys.Remove(enemy);
    }


    public List<EnemyAI> Enemies { get { return m_enemys; } }
    void UpdateWorldContext()
    {
        //Scan a large bound around the unit and updates the worlds context and states
        //for each collider within the bound, catagorise its type, then update the type context

        //check alive enemies & remove any that are dead
        

        //check a 100m bound around player for threats & los & teammate positions


        //if all enemies are dead exit combat
    }


    void UpdateEnemyLOS()
    {

    }

    void UpdateCombatStatus()
    {
        if(m_aliveEnemys.Any())
        {
            foreach(var e in m_aliveEnemys)
            {
                //if(e.)
            }
        }
    }
    #endregion
}
