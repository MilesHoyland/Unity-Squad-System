using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRayTracer : MonoBehaviour
{
    private SquadDirector navigation;
    private GameObject gameObject;
    public float line_scalar = 5f;
    public Color color = Color.white;

    private DebugRayTracer(GameObject go, float scalar, Color col)
    {
        gameObject = go;
        line_scalar = scalar;
        color = col;
    }
    private DebugRayTracer(GameObject go, Color col)
    {
        gameObject = go;
        color = col;
    }

    void Start()
    {
        
    }

    void Render()
    {
        //Draw an itial ray up from the agent GameObject
        Debug.DrawRay(this.transform.position, this.transform.up, color);

        //Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.up * line_scalar), Color.red);
        //Debug.DrawLine(this.transform.position, this.transform.position + (this.transform.up * line_scalar), Color.red, 2);
    }
}
