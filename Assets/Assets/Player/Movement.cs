using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Movement.cs
 * Nicolas Kaplan (301261925) 
 * 2024-01-31
 * Movement for Player
 * V 1.0
 */
public class Movement : MonoBehaviour
{
    [Header("Respawning")]
    [SerializeField] Transform respawnLocation;

    [Header("Character Controller")]
    [SerializeField] CharacterController controller;
   
    [Header("Movement")]
    [SerializeField] float speed = 11f;
    Vector2 horizontalInput;

    [Header("Jumping")]
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
    private void Start()
    {
        originalGravity = gravity;
        Debug.Log($"Initial Original Gravity: {originalGravity}");
    }
    private void Update()
    {
        float halfHeight = controller.height * 0.5f;
        Vector3 bottomPoint = transform.TransformPoint(controller.center - Vector3.up * halfHeight);
        isGrounded = Physics.CheckSphere(bottomPoint, 0.1f, groundMask);

        if (isGrounded)
        {
            Debug.Log("resetting velocity");
            verticalVelocity.y = 0;
        }
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * speed;
        controller.Move(horizontalVelocity * Time.deltaTime);

        if (jump)
        {
            if (isGrounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            jump = false;
        }
        if (hover)
        {
            verticalVelocity.y = -1;
            gravity = gravity / hoverDivider;
            Debug.Log($"Hovering... Gravity: {gravity}");
        }
        else
        {
            if (!isGrounded && !jump)
            {
                gravity = originalGravity;
                Debug.Log("Gravity Normal. Gravity: {gravity}. No longer hovering");
            }
        }
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
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
        gravity = originalGravity;
        hover = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
        {
            controller.enabled = false;
            gameObject.transform.position = respawnLocation.position;
            Debug.Log($"Respawning Player at: {respawnLocation.position}");
            verticalVelocity = Vector3.zero;
            controller.enabled = true;

        }
    }
}
