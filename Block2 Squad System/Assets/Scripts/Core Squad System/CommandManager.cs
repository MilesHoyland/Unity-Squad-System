using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CommandState
{
    HOLDING = 1,
    PUSHING,
    GOINGTO,
    REGROUPING
}

public class CommandManager : MonoBehaviour
{
    #region Private Members
    [SerializeField] SquadManager m_squadManager; 
    [SerializeField] GameObject m_targetIndicator;
    [SerializeField] float m_commandRadius = 5f;
    [SerializeField] SystemUI m_uI;
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
        if(!m_uI)
        {
            Debug.LogError("UI not referenced by script.");
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GoToLocation();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
           // Debug.Log("Execute Squad Command: Go to Location");


        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
           Debug.Log("Execute Squad Command: Regroup");
           CallCommand(CommandState.REGROUPING);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Execute Squad Command: Hold position");
            CallCommand(CommandState.HOLDING);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Execute Squad Command:  Aggressive Push");
            CallCommand(CommandState.PUSHING);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CallCommand(CommandState.GOINGTO);
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
    void CallCommand(CommandState command)
    {
        switch (command)
        {
            case CommandState.GOINGTO:
                {
                    m_uI.UpdateSquadState("MOVING TO POSITION");
                    GoToLocation();
                    break;
                }
            case CommandState.HOLDING:
                {
                    m_uI.UpdateSquadState("HOLDING");
                    HoldPoint();
                    break;
                }
            case CommandState.PUSHING:
                {
                    m_uI.UpdateSquadState("PUSHING");
                    Push();
                    break;
                }
            case CommandState.REGROUPING:
                {
                    m_uI.UpdateSquadState("REGROUPING");
                    Regroup();
                    break;
                }
            default:
                break;
        }
        
    }

    private void Regroup()
    {
        foreach (SquadMemberAI sm in m_squadManager.Squad.Squadies)
        {
            if (sm.Agent)
            {
                sm.HasCommand = false;
            }
        }
    }

    private void GoToLocation()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
        {
            Instantiate(m_targetIndicator, hitInfo.point, m_targetIndicator.transform.rotation);

            foreach (SquadMemberAI sm in m_squadManager.Squad.Squadies)
            {
                //TODO implement navmesh control
                if (sm.Agent)
                {
                    Vector3 point = m_squadManager.SquadController.GetPointInSphere(hitInfo.point, 5f, 10);
                    sm.Agent.SetDestination(point);
                    sm.HasCommand = true;
                }

            }
        }
    }

    private void HoldPoint()
    {
        foreach(SquadMemberAI sm in m_squadManager.Squad.Squadies)
        {
            sm.HasCommand = true;
            sm.Agent.ResetPath();
        }
    }

    private void Push()
    {

    }
    #endregion
}
