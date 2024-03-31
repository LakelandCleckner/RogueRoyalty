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


/// <summary>
/// This class manages the damage done to the player by the Troll's axe
/// </summary>
public class TrollAxe : MonoBehaviour
{
    private bool _hitPlayer = false;

    ///<summary>
    /// When the axe hits the player decrement the player health.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //if we hit the player collider... 
        if (other.gameObject.CompareTag("Player"))
        {
            //check if havent hit the player yet
            if (_hitPlayer == false)
            {
                //decrement the player health
                if (other.gameObject.GetComponent<PlayerHealth>() != null)
                {
                    other.gameObject.GetComponent<PlayerHealth>().LoseHealth();
                }
                //make sure we can't hit the player again.
                _hitPlayer = true;
            }
        }
    }
    
    /// <summary>
    /// When the axe exits the player hitbox, reset _hitPlayer bool
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        _hitPlayer = false;
    }
}
