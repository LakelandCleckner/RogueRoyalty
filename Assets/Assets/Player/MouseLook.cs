using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* MouseLook.cs
 * Nicolas Kaplan (301261925) 
 * 2024-01-31
 * Mouse Lookaround script for player movement.
 * V 1.0
 */
public class MouseLook : MonoBehaviour
{
    [Header("Cursor Options")]
    public bool isCursorLocked;

    [SerializeField] float sensitivityX = 8f; 
    [SerializeField] float sensitivityY = 0.5f;
    float mouseX, mouseY;
    
    [Header("Camera Options")]
    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 85f; // to prevent the player from looking more than directly up or down
    float xRotation = 0f;

    private void Update()
    {
        if (isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
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
