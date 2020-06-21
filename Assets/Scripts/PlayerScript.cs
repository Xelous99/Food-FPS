using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //General  Variables
    static GameObject player;

    static Camera playerCam;

    Transform groundCheck;
    float groundDistance = .4f;
    float gravity = -9.81f;
    float jumpHeight = 4f;

    public LayerMask groundMask;

    bool isGrounded = false;
    

    //Player Variables
        //Player view variables
    float mouseX;
    float mouseY;
    float mouseSensitivity = 100f;
    float xRotation = 0f;

    //Player body variables
    float x;
    float z;
    float playerSpeed = 10f;
    public CharacterController controller;
    Vector3 velocity;

    private Transform playerBody;

    void Start()
    {
        playerCam = gameObject.transform.Find("PlayerView").GetComponent<Camera>();
        playerCam.enabled = true;
        player = gameObject;
        playerBody = player.transform;
        controller = player.GetComponent<CharacterController>();
        groundCheck = player.transform.Find("GroundCheck").transform;
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
        rotatePlayer();
    }

    //Moves the player object 
    private void movePlayer() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2;
        }

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        //Movement
        Vector3 playerMovent = transform.right * x + transform.forward * z;
        controller.Move(playerMovent * playerSpeed * Time.deltaTime);

        //Jumping
        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Falling
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void rotatePlayer() {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerBody.Rotate(Vector3.up * mouseX);
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
