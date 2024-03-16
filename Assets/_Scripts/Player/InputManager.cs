using UnityEngine;
/* InputManager.cs
 * Nicolas Kaplan (301261925) 
 * 2024-01-31
 * 
 * Last Modified by: Alexander Maynard
 * Last Modified Date: 2024-03-15
 *
 * InputManager in charge of player movement, jumping.
 * added functionality to manage inputs with shooting mechanic in mouselook.
 * 
 *  Version History:
 *      -> March 15th, 2024 (by Alexander Maynard):
 *          - added a delay between shots for the player shooting.
 * 
 * V 1.1
 */

public class InputManager : MonoBehaviour
{
    [Header("Dependant Scripts")]
    [SerializeField] Movement movement;
    [SerializeField] MouseLook mouseLook;
    PlayerActions controls;
    PlayerActions.GroundMovementActions groundMovement;

    [Header("Time between shots variables")]
    [SerializeField] private float _totalTimeBetweenShots = 1.0f;
    [SerializeField] private float _timeBetweenShots;

    Vector2 horizontalInput;
    Vector2 mouseInput;
    private void Awake()
    {
        _timeBetweenShots = _totalTimeBetweenShots;

        controls = new PlayerActions();
        groundMovement = controls.GroundMovement;

        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        groundMovement.Jump.performed += _ => movement.OnJumpPressed();
        groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        groundMovement.Fire.performed += _ =>
        {
            if(_timeBetweenShots < 0)
            {
                mouseLook.OnFirePressed();
                _timeBetweenShots = _totalTimeBetweenShots;
            }
            
        };
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

        //decrement the shot time.
        _timeBetweenShots -= 1 * Time.deltaTime;
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
