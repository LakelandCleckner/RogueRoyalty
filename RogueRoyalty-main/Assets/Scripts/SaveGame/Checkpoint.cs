using UnityEngine;
/* Checkpoint.cs
 * Lakeland Cleckner (301344797) 
 * 2024-02-25
 * 
 * Last Modified Date: 2024-02-25
 * Last Modified by: Lakeland Cleckner
 * 
 * 
 * Version History:
 *      -> February 25th, 2024 (Lakeland Cleckner)
 *          - Created script for checkpoint collision. 
 *       -> March 17th 2024 (Lakeland Cleckner)
 *          -Modified OnTriggerEnter to include SetRespawnLocation
 * 
 * 
 *Checkpoint Saving Collider Behaviour
 * V 1.0
 */

public class Checkpoint : MonoBehaviour
{
    // Inside checkpoint collision method
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Update the respawnLocation to the current checkpoint's position
            Movement playerMovement = other.GetComponent<Movement>();
            if (playerMovement != null)
            {
                playerMovement.SetRespawnLocation(transform.position);
            }

            SaveGameManager.Instance.SaveGame(other.transform);
            Debug.Log("Checkpoint reached and game saved.");
        }
    }

}
