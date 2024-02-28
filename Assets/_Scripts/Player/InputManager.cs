using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* InputManager.cs
 * Nicolas Kaplan (301261925) 
 * 2024-01-31
 * InputManager in charge of player movement, jumping.
 * added functionality to manage inputs with shooting mechanic in mouselook.
 * V 1.1
 */

public class InputManager : MonoBehaviour
{
    [Header("Dependant Scripts")]
    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    PlayerActions controls;
    PlayerActions.GroundMovementActions groundMovement;

    Vector2 horizontalInput;
    Vector2 mouseInput;
    private void Awake()
    {
        controls = new PlayerActions();
        groundMovement = controls.GroundMovement;

        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        groundMovement.Jump.performed += _ => movement.OnJumpPressed();
        groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        groundMovement.Fire.performed += _ => mouseLook.OnFirePressed();
    }
    private void Update()
    {
        movement.RecieveInput(horizontalInput);
        mouseLook.RecieveInput(mouseInput);
        // Hovering Check (player needs to press and hold the jump key, be in mid-air, and be going downwards)
        if (groundMovement.Jump.IsPressed() && movement.verticalVelocity.y < 0 && !movement.isGrounded)
        {
            movement.Hovering();
        }
        else
        {
            movement.NotHovering();
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
