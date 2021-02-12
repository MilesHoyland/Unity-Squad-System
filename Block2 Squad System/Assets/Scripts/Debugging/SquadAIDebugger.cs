using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadAIDebugger : MonoBehaviour
{
    public float lineScalar = 1.5f;

    public bool drawLocals = true;
    public bool drawLineOfSight = true;

    //HeightOffset is the offset above a char
    [SerializeField]
    private Vector3 heightOffset = Vector3.up;

    Color red = Color.red;
    Color green = Color.green;
    Color blue = Color.blue;

    //cache the character brains
    private SquadMemberAI[] squadAgents;

    [SerializeField]
    private Color color = Color.green;

    [SerializeField]
    private GUIStyle style;

    void Start()
    {
    }




    void OnDrawGizmos()
    {
        style.normal.textColor = color;

        if (squadAgents == null)
        {
            squadAgents = FindObjectsOfType<SquadMemberAI>();
        }

        foreach (var agent in squadAgents)
        {
            //string persTxt = allAgents.returnDebugData;
            SquadState state = agent.State;
            string text = "Null";
            if(state == SquadState.FOLLOW)
            {
                text = "FOLLOW\n";
            }
            else if(state == SquadState.FIGHT)
            {
                text = "FIGHT\n";
            }
            else { }

            UnityEditor.Handles.Label(agent.transform.position + heightOffset, text);


            //Local Axis Debug
            Gizmos.color = Color.green;
            Gizmos.DrawLine(agent.transform.position, agent.transform.position + agent.transform.up * lineScalar);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(agent.transform.position, agent.transform.position + agent.transform.forward * lineScalar);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(agent.transform.position, agent.transform.position + agent.transform.right * lineScalar);

        }

    }
}
