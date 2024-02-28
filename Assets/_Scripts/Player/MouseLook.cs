using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* MouseLook.cs
 * Author: Nicolas Kaplan (301261925) 
 * 2024-01-31
 * 
 * Last Modified Date: January 31st, 2024
 * Last Modified by: Alexander Maynard
 * 
 * Revision History:
 *      -> February 1st, 2024:
 *          -Refactored the cursor lock mode functionality. 
 *          This included removing it from the Update 
 *          function, removing the variable (with if else) and 
 *          putting it in the OnAwake function.
 * 
 * 
 * 
 * Mouse Lookaround script for player movement.
 * V 1.1
 */
public class MouseLook : MonoBehaviour
{
    [Header("Cursor Options")]
    [SerializeField] float sensitivityX = 8f; 
    [SerializeField] float sensitivityY = 0.5f;
    float mouseX, mouseY;
    
    [Header("Camera Options")]
    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 85f; // to prevent the player from looking more than directly up or down
    float xRotation = 0f;

    /// <summary>
    /// set cursor lock state to Locked on Awake for playability.
    /// </summary>
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, mouseX * Time.deltaTime);

        // if bool invertedAxis == true (Settings)
        //{ xRotation += mouseY; } (could be extra functionality added later)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;
    }
    public void RecieveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;
    }
}
