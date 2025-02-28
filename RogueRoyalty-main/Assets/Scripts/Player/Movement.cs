using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* Movement.cs
 * Nicolas Kaplan (301261925) 
 * 2024-01-31
 * 
 * Last Modified Date: 2024-03-16
 * Last Modified by: Nicolas Kaplan
 * 
 * Version History:
 *      -> February 4th, 2024
 *          - Added temporary call to the GameOver scene in the OnTriggerEnter for Assignment 1 - Part 2 for now;
 *          instead of returning the player to the respawn point.
 *      -> February 21st, 2024
 *          - Removed temporary call to GameOver scene, and added public void SendToCheckpoint() to be called from the new
 *            PlayerHealth.cs script
 *      -> March 15th, 2024 (by Alexander Maynard)
 *          - Commented out the Debug.Log() lines
 *      -> March 16th, 2024 (Nick Kaplan)
 *          - Created mobile functionality for movement and jumping.
 *      -> March 17th, 2024 (Lakeland Cleckner)
 *          - Added SetRespawnLocation for checkpoint functionality
 * Movement for Player
 * V 1.3
 */
public class Movement : MonoBehaviour
{
    [Header("Respawning")]
    [SerializeField] Transform respawnLocation;

    [Header("Character Controller")]
    [SerializeField] CharacterController controller;

    [Header("Movement")]
    [SerializeField] Joystick movementJoystick;
    [SerializeField] float speed = 11f;
    Vector2 horizontalInput;

    [Header("Jumping")]
    [SerializeField] Button jumpButton;
    [SerializeField] float jumpHeight = 3.5f;
    public bool jump;

    [Header("Hovering")]
    [SerializeField] float hoverDivider = 4f; // how much are we dividing the player's gravity by when they hover?
    public bool hover;

    [Header("Gravity & Velocity")]
    [SerializeField] float gravity = -30f;
    float originalGravity;
    public Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    public bool isGrounded;

    [Header("Sound")]
    private AudioSource audioSource;
    [SerializeField] AudioClip walkingSound; // Attach your walking sound clip here in the inspector

    private void Start()
    {
        originalGravity = gravity;
        audioSource = GetComponent<AudioSource>();
        jumpButton.onClick.AddListener(OnJumpPressed);
    }

    private void Update()
    {
        controller.Move(new Vector3(movementJoystick.Direction.x * speed * Time.deltaTime, 0, movementJoystick.Direction.y * speed * Time.deltaTime));
        float halfHeight = controller.height * 0.5f;
        Vector3 bottomPoint = transform.TransformPoint(controller.center - Vector3.up * halfHeight);
        isGrounded = Physics.CheckSphere(bottomPoint, 0.1f, groundMask);

        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed;
        controller.Move(horizontalVelocity * Time.deltaTime);

        if (jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
                jump = false;
            }
        }

        if (hover)
        {
            verticalVelocity.y = -1;
            gravity = gravity / hoverDivider;
        }
        else
        {
            gravity = originalGravity;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        // Play walking sound when moving
        if (horizontalVelocity.magnitude > 0.1f && isGrounded)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = walkingSound;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void RecieveInput(Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
    }

    public void OnJumpPressed()
    {
        jump = true;
    }

    public void Hovering()
    {
        hover = true;
    }

    public void NotHovering()
    {
        hover = false;
        gravity = originalGravity;
    }

    public void SendToCheckpoint()
    {
        controller.enabled = false;
        gameObject.transform.position = respawnLocation.position;
        verticalVelocity = Vector3.zero;
        controller.enabled = true;
    }

    public void SetRespawnLocation(Vector3 newPosition)
    {
        if (respawnLocation != null)
        {
            respawnLocation.position = newPosition;
        }
        else
        {
            Debug.LogError("Respawn location transform is not set.");
        }
    }
}
