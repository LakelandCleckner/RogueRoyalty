using UnityEngine;
using UnityEngine.UI;
/* InputManager.cs
* Nicolas Kaplan (301261925) 
* 2024-01-31
* 
* Last Modified by: Alexander Maynard
* Last Modified Date: 2024-04-16
*
* InputManager in charge of player movement, jumping.
* added functionality to manage inputs with shooting mechanic in mouselook.
* 
*  Version History:
*       -> March 15th, 2024 (by Alexander Maynard):
*          - added a delay between shots for the player shooting.
*          - added functionality for mobile touch button for shooting with the UI button.
*          - added initial aiming joystick controls.
*      
*       -> April 15th, 2024 (by Alexander Maynard):
*           - Removed mouse input for PC as mobile is being used going forward (only wasd and space for PC testing purposes will be kept)
*           
*       -> April 16th, 2024 (by Alexander Maynard):
*           - Removed mouse input for PC as mobile is being used going forward (only wasd and space for PC testing purposes will be kept).
*           -Refactored this script to no longer have to check of when to hover and not hover. This is now just a check for NotHovering when the player is grounded. 
*               This was due to use needing better support for the float mechanic on mobile (Android).
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

    [Header("Mobile Touch Buttons")]
    [SerializeField] private Button _shootBtn;
    [SerializeField] private Joystick _aimingJoystick;

    Vector2 horizontalInput;
    Vector2 mouseInput;
    private void Awake()
    {
        //to control how much time between each time the player shoots
        _timeBetweenShots = _totalTimeBetweenShots;

        controls = new PlayerActions();
        groundMovement = controls.GroundMovement;
        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        groundMovement.Jump.performed += _ => movement.OnJumpPressed();

        //mobile touch button for shooting
        _shootBtn.onClick.AddListener(() =>
        {
            if(_timeBetweenShots < 0)
            {
                mouseLook.OnFirePressed();
                _timeBetweenShots = _totalTimeBetweenShots;
            }
        });
    }
    private void Update()
    {
        mouseInput.x = _aimingJoystick.Direction.x;
        mouseInput.y = _aimingJoystick.Direction.y;

        movement.RecieveInput(horizontalInput);
        mouseLook.RecieveInput(mouseInput);

        if(movement.isGrounded)
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
