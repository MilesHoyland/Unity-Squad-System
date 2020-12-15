using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//The actual squad manager that holds the squad and the behavioural data.
//I imagine this would be similar to the blackboard in a behaviour tree.
public class Squad : MonoBehaviour
{
    public SquadMemberAI[] squad;
    public Navigation navigation;

    private void Start()
    {
        //Initializing the squad
        foreach (SquadMemberAI sm in squad)
        {
            GameObject go = sm.GetComponent<GameObject>();
            if(go)
            { 
                //Place on a random location on the navmesh
                go.gameObject.transform.Translate(navigation.GetPointInSphere(gameObject.transform.position , 5f, 30));
            
            }
        }
        ResolveSquad();
    }

    public void ResolveSquad()
    {
        int maxIterators = 30;

        foreach(SquadMemberAI sm in squad)
        {
            Collider smCollider = sm.GetComponent<Collider>();
            
            foreach (SquadMemberAI fellow_sm in squad)
            {
                Collider fellowCollider = fellow_sm.GetComponent<Collider>();
                //Place on a random location on the navmesh
                if (fellowCollider.bounds.Contains(sm.transform.position)) 
                {
                    sm.gameObject.transform.Translate(navigation.GetPointInSphere(gameObject.transform.position, 5f, maxIterators));
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
