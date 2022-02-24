using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Need both camera and controller, make sure to drag camera in inspector!
    [SerializeField] Transform cameraTransform;
    CharacterController playerController;
    
    // Direction player wants to head on X-Z plane (defined by inputs)
    Vector2 playerDir = new Vector2();
    // accelDir used for smooth damping laters
    Vector2 accelDir = new Vector2();

    // X, Y, and Z components of total player velocity
    Vector3 x, y, z = new Vector3();

    float cameraPitch = 0.0f;

    public float gravity = 9.8f;
    public float jumpPower = 10.0f;
    bool doubleJump = true;

    public float maxSprintTime = 1.0f;  // Maximum time player is allowed to sprint for
    public float currSprintTime = 0.0f; // Timer to keep track of how long player has been sprinting

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        currSprintTime = maxSprintTime;             // On start-up, player has 100% stamina
        Cursor.lockState = CursorLockMode.Locked;   // Lock cursor so it doesn't leave game window
    }

    // Called once at the start of every frame
    void Update()
    {
        // Step 1: Adjust camera
        // Get mouse input
        Vector2 mouseVel = new Vector2();
        mouseVel.x = Input.GetAxis("Mouse X");
        mouseVel.y = Input.GetAxis("Mouse Y");

        cameraPitch -= mouseVel.y * 2.0f;                       // Adjust camera pitch by input and sensitivity
        cameraPitch = Mathf.Clamp(cameraPitch, -85.0f, 85.0f);  // Clamp camera pitch so player can only look up and down by 85 degrees

        cameraTransform.localEulerAngles = Vector3.right * cameraPitch; // Adjust camera pitch
        transform.Rotate(Vector3.up * mouseVel.x * 2.0f);               // Rotate player (camera is child of player so camera rotates too)

        // Step 2: Adjust speed
        // Movespeed, is higher is sprinting
        float speed = 10.0f;
        if (Input.GetButton("Shift")) {
            currSprintTime -= Time.deltaTime;               // Take from sprint time
            currSprintTime = Mathf.Max(currSprintTime, 0);  // Make sure sprint time is never below 0
            if (currSprintTime > 0)                         // If player can sprint then double speed
                speed *= 2;
        }
        else {
            // Otherwise add sprint time if more can be added
            if (currSprintTime < maxSprintTime)
                currSprintTime += Time.deltaTime;
        }

        // Step 3: X - Z plane velocity
        // Get keyboard/controller input
        Vector2 inputDir = new Vector2();
        inputDir.x = Input.GetAxis("Vertical");
        inputDir.y = Input.GetAxis("Horizontal");
        inputDir.Normalize();   // Normalise cause we only care about direction
        playerDir = Vector2.SmoothDamp(playerDir, inputDir, ref accelDir, 0.2f);    // Adds to de/acceleration

        // Calculate X and Z components of total velocity
        x = (playerDir.y * transform.right) * speed;
        z = (playerDir.x * transform.forward) * speed;

        // Step 4: Y axis velocity
        // Check if player is touching the ground
        if (playerController.isGrounded) {
            y.y = 0;            // Reset Y velocity
            doubleJump = true;  // Allow player to double jump again
            if (Input.GetButtonDown("Jump"))
                y.y = jumpPower;
        }
        else if (Input.GetButtonDown("Jump") && doubleJump) {
            y.y = jumpPower;
            doubleJump = false;
        }
        // Apply gravity to Y velocity
        y -= Vector3.up * gravity * Time.deltaTime;
        
        // Calculate total player velocity
        Vector3 playerVel = x + y + z;
        playerController.Move(playerVel * Time.deltaTime);  // Apply with delta time for smooth motion
    }

}