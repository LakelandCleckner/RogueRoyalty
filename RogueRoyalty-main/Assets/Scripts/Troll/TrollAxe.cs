/*
 * Source File Name: TrollAxe.cs
 * Author Name: Alexander Maynard
 * Student Number: 301170707
 * Creation Date: February 28th, 2024
 * Last Modified by: Alexander Maynard
 * Last Modified Date: February 28th, 2024
 * 
 * 
 * Program Description: This script handles decrementing the player health when the player is hit by the Troll's Axe
 *      
 * 
 * 
 * Revision History:
 *      -> February 28th, 2024:
 *          -Implemented all functionality for this script.
 *          -Created all proper documentation.
 */

using UnityEngine;

public class TrollAxe : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.LoseHealth();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // This can be used to reset any state if necessary
    }
}