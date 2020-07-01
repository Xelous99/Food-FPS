using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    //General  Variables
    static GameObject player;
    public GunScript primary;
    RaycastHit sight;

    [SerializeField]
    Canvas UI;

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

    [SerializeField]
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
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
        rotatePlayer();
        look();
        checkForActions();
        if (Input.GetMouseButton(0) && primary != null)
        {
            primary.shoot();
        }
    }

    private void checkForActions() {
        if (Input.GetKeyDown(KeyCode.F) && sight.transform.gameObject.tag == "Weapon") {
            primary = sight.transform.gameObject.GetComponent<GunScript>();
            primary.equipt(player);
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            primary.switchWeapon(!primary.IsEquipted);
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            primary.switchWeapon(true);
            primary.dropWeapon();
            primary = null;
        }

        
    }

    //Sends out a constant raycast to see what the player is looking at.
    private void look() {
        
        Text UIText;
        UIText = UI.transform.Find("Text").GetComponent<Text>();
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out sight, 10f))
        {
            if (sight.transform.gameObject.tag == "Weapon") {
                UIText.enabled = true;
            }
            else
            {
                UIText.enabled = false;
            }

        }
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
