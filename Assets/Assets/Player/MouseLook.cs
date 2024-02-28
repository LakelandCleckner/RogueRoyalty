using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* MouseLook.cs
 * Author: Nicolas Kaplan (301261925) 
 * 2024-01-31
 * 
 * Last Modified Date: February 28th, 2024
 * Last Modified by: Nicolas Kaplan
 * 
 * Revision History:
 *      -> February 1st, 2024:
 *          -Refactored the cursor lock mode functionality. 
 *          This included removing it from the Update 
 *          function, removing the variable (with if else) and 
 *          putting it in the OnAwake function.
 *      -> February 28th, 2024:
 *          - Added shooting functionality using raycasts.
 * 
 * 
 * Mouse Lookaround script for player movement.
 * V 1.2
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

    [Header("Shooting")]
    Ray ray;
    RaycastHit hit;
    [SerializeField] LayerMask layerMask;

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
    public void OnFirePressed()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            if (hit.transform.gameObject.GetComponent<EnemyHealth>() != null)
            {
                hit.transform.gameObject.GetComponent<EnemyHealth>().enemyHealth -= 1;
                Debug.Log($"Hit enemy {hit.transform.gameObject.name}. Health reduced by 1.");
            }
            else if (hit.transform.gameObject.GetComponentInChildren<EnemyHealth>() != null)
            {
                hit.transform.gameObject.GetComponentInChildren<EnemyHealth>().enemyHealth -= 1;
                Debug.Log($"Hit enemy {hit.transform.gameObject.name}. Health reduced by 1.");
            }
            else if (hit.transform.gameObject.GetComponentInParent<EnemyHealth>() != null)
            {
                hit.transform.gameObject.GetComponentInParent<EnemyHealth>() .enemyHealth -= 1;
                Debug.Log($"Hit enemy {hit.transform.gameObject.name}. Health reduced by 1.");
            }
            else
            {
                Debug.Log($"Whatever this ray hit does not contain health. {hit.transform.gameObject.name}");
            }
        }
        else
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
        }
    }
}
