using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommandManager : MonoBehaviour
{
    #region Private Members
    [SerializeField] SquadManager m_squadManager; 
    [SerializeField] GameObject m_targetIndicator;
    [SerializeField] float m_commandRadius = 5f;
    #endregion

    #region Properties
    public SquadManager Manager { set { m_squadManager = value; } }
    #endregion

    #region Main Methods
    private void Start()
    {
        if (!m_targetIndicator)
        {
            Debug.LogError("Command Indicator not set up properly.");
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Instantiate(m_targetIndicator, hitInfo.point, m_targetIndicator.transform.rotation);
                foreach (SquadMemberAI sm in m_squadManager.Squad.squad)
                {
                    //TODO implement navmesh control
                    if (sm.nav_agent)
                    {
                        Vector3 point = m_squadManager.SquadDirector.GetPointInSphere(hitInfo.point, 5f, 10);
                        sm.nav_agent.SetDestination(point);
                    }

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
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

        if (Input.GetKeyDown(KeyCode.Q))
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
    #endregion

    #region Utility Methods
    private void Regroup()
    {
        foreach (SquadMemberAI sm in m_squadManager.Squad.squad)
        {
            //Change state in SquadMemberAI to Regroup
        }
    }

    private void GoToLocation()
    {
        foreach (SquadMemberAI sm in m_squadManager.Squad.squad)
        {
            //Change state in SquadMemberAI to Regroup
        }
    }

    private void HoldPoint()
    {

    }

    private void Push()
    {

    }
    #endregion
}
