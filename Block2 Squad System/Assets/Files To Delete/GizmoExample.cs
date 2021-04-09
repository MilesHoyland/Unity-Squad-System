using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoExample : MonoBehaviour
{

    #region Public Variables
    public Color gizmosColor;
    public Color gizmosLineColor;
    
    public float gizmoSize;
    public float radius = 1f;

    public Vector3 posA;
    public Vector3 posB;
    #endregion

    #region Private Variables
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
    void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawSphere(transform.position, radius);

        Gizmos.color = gizmosLineColor;
        Gizmos.DrawWireSphere(transform.position, radius);

        //Gizmos.color = Color.green;
        //Gizmos.DrawRay(transform.position, transform.up);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawRay(transform.position, transform.forward);

        //Gizmos.color = Color.red;
       // Gizmos.DrawRay(transform.position, transform.right);
    }

    void OnDrawGizmosSelected()
    {

    }
    #endregion
}
