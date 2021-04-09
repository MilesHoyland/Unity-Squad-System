using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//The actual squad manager that holds the squad and the behavioural data.
//I imagine this would be similar to the blackboard in a behaviour tree.
public class Squad : MonoBehaviour
{
    [SerializeField] SquadMemberAI[] m_squad;
    [SerializeField] SquadController m_controller;

    public SquadMemberAI[] Squadies { get { return m_squad; } }

    //TODO: Refactor this code to be executed by the squad director instead of on the start
    private void Start()
    {
        //Initializing the squad
        bool needResolve = false;

        SquadMemberAI[] memberAIs = this.gameObject.GetComponentsInChildren<SquadMemberAI>();

        m_squad = memberAIs;

        foreach (SquadMemberAI sm in m_squad)
        {
            GameObject go = sm.gameObject;
            Collider smCollider = go.GetComponent<Collider>();
            foreach (SquadMemberAI fellow_sm in m_squad)
            {
                if(fellow_sm == sm )
                {
                    break;
                }
                GameObject fellow_go = fellow_sm.gameObject;
                Collider fellowCollider = fellow_go.GetComponent<Collider>();
                if (fellowCollider.bounds.Contains(smCollider.transform.position))
                {
                    needResolve = true;
                    break;
                }
            }
        }

        if(needResolve)
        {
            ResolveSquad();
        }
    }

    public void ResolveSquad()
    {
        int maxIterators = 30;

        foreach(SquadMemberAI sm in m_squad)
        {
            Collider smCollider = sm.GetComponent<Collider>();
            
            foreach (SquadMemberAI fellow_sm in m_squad)
            {
                Collider fellowCollider = fellow_sm.GetComponent<Collider>();
                //Place on a random location on the navmesh
                if (fellowCollider.bounds.Contains(sm.transform.position)) 
                {
                    sm.gameObject.transform.Translate(m_controller.GetPointInSphere(gameObject.transform.position, 5f, maxIterators));
                }
            }
        }
        Debug.Log("Squad - Reposition Squad carried out max iterations.");
    }

    public void AllocateMembers()
    {
        //TODO: used to initialise

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
