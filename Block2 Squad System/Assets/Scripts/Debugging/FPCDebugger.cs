using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMovement))]
public class FPCDebugger : GizmoExample
{

    #region Public Variables
    public PlayerMovement playerMovement;

    public Color groundCheckColour;
    public Color hitColour;
    #endregion

    #region Private Variables
    #endregion

    #region Main Methods
    private void Awake()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            Debug.Log("playermovement is not null");
        }
    }
    #endregion

    #region Utility Methods
    void OnDrawGizmos()
    {
        if(playerMovement)
        {

        //Local Axis Debug
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.up);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right);


        //Ground Check Debug
        Gizmos.color = groundCheckColour;
        if(playerMovement.GetGrounded())
        {
            Gizmos.color = hitColour;
        }
        Gizmos.DrawWireSphere(playerMovement.groundCheck.position, playerMovement.groundDistance);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward);

        }
    }

    private void OnDrawGizmosSelected()
    {

    }
    #endregion
}
