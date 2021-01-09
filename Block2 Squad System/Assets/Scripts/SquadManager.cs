using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquadManager : MonoBehaviour
{
    #region Manager
    [SerializeField] SystemManager system;
    #endregion

    #region Subsystems
    [SerializeField] CommandManager m_commandManager; 
    [SerializeField] SquadDirector m_squadDirector;
    [SerializeField] WorldIntelligence m_worldIntelligence;
    #endregion

    #region Private Members
    [SerializeField] FormationManager m_formationManager;
    [SerializeField] Squad m_squad;


    //cache reference to squadmate gameobjects
    [SerializeField] GameObject[] m_smates;
    #endregion

    #region Properties
    public FormationManager FormationManager { get { return m_formationManager; } }
    public Squad Squad { get { return m_squad; } }
    public SquadDirector SquadDirector { get { return m_squadDirector; } }
    public WorldIntelligence WorldIntelligence { get { return m_worldIntelligence; } }

    #endregion

    #region Main Methods
    void Start()
    {   
        //TODO change to get from squad system script
        if(m_squad)
        {
            m_smates = new GameObject[m_squad.squad.Length];
            for(int i = 0; i < m_squad.squad.Length; i++)
            {
                m_smates[i] = m_squad.squad[i].gameObject;
                Debug.Log("Squad mate: " + m_squad.squad[i].name + " was added to the squad controller.");
            }
        }
        if (!m_squad)
        {
            Debug.LogError("Squad instance not properly linked.");
        }
        if(!m_squadDirector)
        {
            Debug.LogError("Navigation not properly referenced.");
        }
        if(!m_formationManager)
        {
            Debug.LogError("Formation manager not properly referenced.");
        }
        if(!system)
        {
            Debug.LogError("System manager not properly referenced.");
        }
        if(!m_commandManager)
        {
            m_commandManager = gameObject.GetComponent<CommandManager>();
            if (!m_commandManager)
            {
                Debug.LogError("commandManager not properly linked.");
            }
            else
            {
                m_commandManager.Manager = this;
            }
        }

    }

    void Update()
    {

    }
    #endregion

    #region Utility Methods
    #endregion
}

