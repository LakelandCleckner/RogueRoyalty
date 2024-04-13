using UnityEngine;
using UnityEngine.UI;

/* InputManager.cs
* Author: Nicolas Kaplan (301261925) 
* 2024-01-31
* Last Modified by: Alexander Maynard
* Last Modified Date: 2024-03-15
*
* InputManager in charge of player movement, jumping, and shooting.
* V 1.2
*/

public class InputManager : MonoBehaviour
{
    [Header("Dependant Scripts")]
    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    PlayerActions controls;
    PlayerActions.GroundMovementActions groundMovement;

    [Header("Time between shots variables")]
    [SerializeField] private float totalTimeBetweenShots = 1.0f;
    private float timeSinceLastShot;

    [Header("Mobile Touch Buttons")]
    [SerializeField] private Button shootBtn;
    [SerializeField] private Joystick aimingJoystick;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;  // Attach your AudioSource here
    [SerializeField] private AudioClip shootingSound;  // Drag your shooting sound clip here in the inspector

    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        timeSinceLastShot = totalTimeBetweenShots; // Initialize so the player can shoot immediately

        controls = new PlayerActions();
        groundMovement = controls.GroundMovement;
        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        groundMovement.Jump.performed += _ => movement.OnJumpPressed();
        groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();

        // Mobile touch button for shooting
        shootBtn.onClick.AddListener(Shoot);
    }

    private void Shoot()
    {
        if (timeSinceLastShot >= totalTimeBetweenShots)
        {
            mouseLook.OnFirePressed();
            audioSource.PlayOneShot(shootingSound);
            timeSinceLastShot = 0; // Reset timer
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime; // Always increment the timeSinceLastShot

        mouseInput.x = aimingJoystick.Direction.x;
        mouseInput.y = aimingJoystick.Direction.y;

        movement.RecieveInput(horizontalInput);
        mouseLook.RecieveInput(mouseInput);

        if (groundMovement.Jump.IsPressed() && movement.verticalVelocity.y < 0 && !movement.isGrounded)
        {
            movement.Hovering();
        }
        else
        {
            movement.NotHovering();
        }

        // Keyboard or gamepad fire input
        if (groundMovement.Fire.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
