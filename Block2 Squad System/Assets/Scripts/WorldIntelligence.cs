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
    
    //Points Of Interest & customEvents
    //List<POI> m_poi

    //Items
    //List<Items> m_items;

    #endregion


    #region Main Methods
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    #endregion

    #region Utility Methods
    void UpdateWorldContext()
    {
        //Scan a large bound around the unit and updates the world context and states
        //for each collider within the bound, catagorise its type, then update the type context

        //check alive enemies & remove any that are dead
        


        //check a 100m bound around player for threats & los & teammate positions

    }
    #endregion
}
