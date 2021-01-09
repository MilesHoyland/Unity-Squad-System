using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float runSpeed = 10.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] float jumpHeight = 3f;

    [SerializeField] [Range(0.0f, 0.3f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.3f)] float mouseSmoothTime = 0.03f;

    [SerializeField] bool lockCursor = true;

    float velocityY = 0.0f;

    Vector2 currDir = Vector2.zero;
    Vector2 currDirVelocity = Vector2.zero;

    Vector2 currMouseDelta = Vector2.zero;
    Vector2 currMouseDeltaVelocity = Vector2.zero;

    float cameraPitch = 0f;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    void Update()
    {
        UpdateMouseControls();
        UpdateMoveControls();
    }

    void UpdateMouseControls()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currMouseDelta = Vector2.SmoothDamp(currMouseDelta, targetMouseDelta, ref currMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currMouseDelta.x * mouseSensitivity);
    }

    void UpdateMoveControls()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currDir = Vector2.SmoothDamp(currDir, targetDir, ref currDirVelocity, moveSmoothTime);

        if(controller.isGrounded)
        {
            velocityY = 0.0f;
        }
        if(Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currDir.y + transform.right * currDir.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }
}
