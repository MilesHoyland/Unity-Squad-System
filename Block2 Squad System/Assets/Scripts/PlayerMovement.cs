using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Public Variables
    public bool run = false;

    public CharacterController controller;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    public float speed = 12.0f;
    public float gravity = -9.81f;
    public float jumpHieght = 4.0f;
    #endregion

    #region Private Variables
    private bool isGrounded;
    Vector3 velocity;
    #endregion

    #region Main Methods
    void Update()
    {
        if (run)
        {

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // checks if there are any collisions with colliders on the ground mask
            //Debug.Log("grounded = " + isGrounded);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 movement = transform.right * x + transform.forward * z; // working in local coordinates and vector translation with unitary directions for right and forward

            controller.Move(movement * speed * Time.deltaTime);

            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHieght * -2f * gravity);
                Debug.Log("Jumping");
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }

    public bool GetGrounded()
    {
        return isGrounded;
    }
    #endregion
}
