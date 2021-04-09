using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{

    #region Systems
    [SerializeField] SquadManager squadManager;
    FPSController fpcScript = null;

    [Header("Systems")]
    SquadManager squad_manager;
    CommandManager command_manager;
    WorldIntelligence world_intelligence;
    SquadController squad_controller;
    SquadAIDebugger squad_ai_debugging;
    FormationManager formation_manager;

    #endregion

    #region Public Members
    public GameObject flyCam;
    public GameObject player;
    public GameObject indicatorPrefab;
    #endregion

    #region Private Members
    [SerializeField] ControlMode controlMode = ControlMode.FPS;

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
            if (!fpcScript)
            {
                fpcScript = player.GetComponent<FPSController>();
            }
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
        if ((ControlMode)controlMode == ControlMode.FlyCam)
        {
            //Debug.Log("Control mode changes to FPS.");
            player.gameObject.SetActive(true);
            flyCam.gameObject.SetActive(false);
            controlMode = ControlMode.FPS;
            return;
        }
        if ((ControlMode)controlMode == ControlMode.FPS)
        {
            //Debug.Log("Control mode changes to Flycam.");
            flyCam.SetActive(true);
            player.SetActive(false);
            controlMode = ControlMode.FlyCam;
            return;
        }
    }
    #endregion
}
