using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{

    #region Systems
    [SerializeField] SquadManager squadManager;
   // [SerializeField] PlayerMovement fpcScript;
    #endregion

    #region Public Members
    public GameObject flyCam;
    public GameObject player;
    public GameObject indicatorPrefab;
    #endregion

    #region Private Members
    [SerializeField] bool isFlyCam = true;
    [SerializeField] ControlMode controlMode = ControlMode.FlyCam;

    enum ControlMode
    {
        FlyCam = 0,
        FPS = 1
    }
    #endregion


    #region Main Methods
    void Start()
    {
        if (player)
        {
 //           fpcScript = player.GetComponent<PlayerMovement>();
  //          if (!fpcScript)
 //           {
  //             Debug.LogError("Referenced player does not own a FPC Script.");
   //         }
         //   else
   //         {
//Debug.Log("Player properly initialised.");
  //          }
        }
        else
        {
            Debug.LogError("Player GameObject not referenced");
        }

        if (flyCam)
        {
            Debug.Log("Fly Cam properly referenced.");
        }
        else
        {
            Debug.LogError("Fly cam not properly referenced.");
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (controlMode.Equals(ControlMode.FPS))
        {

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("****Toggle Control Mode****");
            ToggleControlMode();
        }
    }
    #endregion

    #region UtilityMethods
    private void ToggleControlMode()
    {
        if (isFlyCam)
        {
            flyCam.SetActive(false);
            player.SetActive(true);
            isFlyCam = false;
            controlMode = ControlMode.FPS;
        }
        if (!isFlyCam)
        {
            player.SetActive(false);
            flyCam.SetActive(true);
            isFlyCam = true;
            controlMode = ControlMode.FlyCam;
        }
    }
    #endregion
}
