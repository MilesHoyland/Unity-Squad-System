using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MouseController : MonoBehaviour
{
    #region Public Variables

    [Space]

    [Header("Mouse Settings")]
    [Range(1.0f, 100.0f)]
    public float sensitivity = 100f;
    public float xRotation = 0f;
    public Transform player;  // ref to players transform
    public LayerMask interactionLayer;

    //MENU
    public RingMenuMono mainMenuPrefab;
    protected RingMenuMono mainMenuInstance;


    #endregion


    #region Private Variables   
    private bool isHolding = false;
    private bool mousePaused = false;

    enum CamState
    {
        FLY_CAM = 0,
        FPS_CAM = 1
    }

    private GameObject aimProbePrimitive;
    #endregion

    


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        aimProbePrimitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        aimProbePrimitive.transform.localScale.Set(0.1f, 0.1f, 0.1f);
        aimProbePrimitive.GetComponent<SphereCollider>().enabled = false;
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (mousePaused)
            {
                mousePaused = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                mousePaused = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }



        if (!mousePaused)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            xRotation -= mouseY; // variable for x rotation based on mouse y
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // clamping cameramovement to a 90 fov

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);  // horizontal cam movement


            
            

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, interactionLayer))
                {
                    // if ray is on the interaction layer
                }
            }
        }

    }

    public GameObject GetImmediateG0()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, LayerMask.NameToLayer("Ground")))
            {
                // if ray is on the interaction layer
                return hit.collider.gameObject;
            }
        }
        Debug.Log("No interactable collider found");
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition));
    }
}
