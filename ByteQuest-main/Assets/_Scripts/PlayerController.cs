using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController player;
    public Transform cameraTransform;

    [SerializeField] private float moveSpeed, gravity, fallVelocity, jumpForce;
    private Vector3 axis, movePlayer;

    // Camera variables
    public Transform playerBody;
    public float mouseSensitivity = 100f;
    float xRotation = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<CharacterController>();

        // Lock and hide the cursor to make the camera movement smoother
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement player with camera
        HandleCameraRotation();
        
        axis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(axis.magnitude > 1) axis = transform.TransformDirection(axis).normalized;
        else axis = transform.TransformDirection(axis);
        
        // Player move
        movePlayer.x = axis.x;
        movePlayer.z = axis.z;
        setGravity();
        player.Move(movePlayer * moveSpeed * Time.deltaTime);
    }

    // Gravity player
    private void setGravity()
    {
        if(player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            if(Input.GetKeyDown(KeyCode.Space)) fallVelocity = jumpForce;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
        }
        movePlayer.y = fallVelocity;
    }

    private void HandleCameraRotation()
    {
        // Get mouse inputs for camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player body horizontally
        playerBody.Rotate(Vector3.up * mouseX);

        // Calculate the rotation for vertical camera movement (clamped between -90 and 90 degrees)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the rotation to the camera transform
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
