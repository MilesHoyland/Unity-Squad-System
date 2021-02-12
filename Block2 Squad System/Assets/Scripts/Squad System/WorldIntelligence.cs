using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldIntelligence : MonoBehaviour
{
    #region Sub-Systems
    [SerializeField] SystemManager m_systemManager;
    [SerializeField] SquadManager m_squadManager;
    #endregion

    #region Members
    //Enemys
    List<EnemyAI> m_enemys;
    List<EnemyAI> m_aliveEnemys;

    //Player Data
    [SerializeField] GameObject m_player;
    [SerializeField] bool isPlayerSeen = false;
    [SerializeField] bool inCombat = false;

    //Points Of Interest & customEvents
    //List<POI> m_poi

    //Items
    //List<Items> m_items;

    #endregion

    #region Public Members
    public bool PlayerSeen { get { return isPlayerSeen ; } set { isPlayerSeen = value ; } }
    public bool InCombat { get { return inCombat; } set { inCombat = value; } }
    #endregion

    #region Main Methods
    void Start()
    {
        if (!m_player)
            Debug.LogError("Player not referenced.");
        if (!m_systemManager)
            Debug.LogError("System manager not linked.");
        if (!m_squadManager)
            Debug.LogError("Squad not linked.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inCombat = true;
            Debug.Log("In Combat = true");
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            inCombat = false;

            Debug.Log("In Combat = false");
        }
    }
    #endregion

    #region Utility Methods
    public List<EnemyAI> GetEnemies { get { return m_enemys; } }
    void UpdateWorldContext()
    {
        //Scan a large bound around the unit and updates the world context and states
        //for each collider within the bound, catagorise its type, then update the type context

        //check alive enemies & remove any that are dead
        

        //check a 100m bound around player for threats & los & teammate positions


        //if all enemies are dead exit combat
    }

    //bool CanSeeEnemies()
    #endregion
}
