using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadAIDebugger : MonoBehaviour
{
    SquadMemberAI squadMember;
    Color red = Color.red;
    Color green = Color.green;
    Color blue = Color.blue;

    public bool drawLocals = true;
    public bool drawLineOfSight = true;
    //public bool

    //HeightOffset is the offset above a char
    [SerializeField]
    private Vector3 heightOffset = Vector3.up;

    //cache the character brains
    private SquadMemberAI[] allAgents;

    [SerializeField]
    private Color color = Color.green;

    [SerializeField]
    private GUIStyle style;

    void Start()
    {
        squadMember = this.GetComponent<SquadMemberAI>();
        if(!squadMember)
        {
            Debug.Log("Squad member AI script not found.");
        }

    }




    void OnDrawGizmos()
    {
        style.normal.textColor = color;

        if (allAgents == null)
        {
            allAgents = FindObjectsOfType<SquadMemberAI>();
        }
        foreach (var allAgents in allAgents)
        {
            //string persTxt = allAgents.returnDebugData;
            string text = allAgents.ToString();
            //UnityEditor.Handles.Label(allAgents.transform.position + heightOffset, text);
        }



    }
}
