using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public float walkSpeed = 3f;
    public float runSpeed = 9f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundLayerMask;

    Vector3 velocity;
    bool isGrounded;
    int doubleJumpStack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey("left shift") && Input.GetKey("w"))&&!Input.GetKey(KeyCode.Mouse0))
        {
            Move(runSpeed);
        }
        else
        {
            Move(walkSpeed);
        }
    }
    private void Move(float speed)
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            doubleJumpStack = 2;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        characterController.Move(move * speed * Time.deltaTime);

        if (doubleJumpStack != 0 && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(gravity * jumpHeight * -2f);
            doubleJumpStack--;
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}
