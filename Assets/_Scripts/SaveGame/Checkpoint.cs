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
 *      -> February 25th, 2024
 *          - Created script for checkpoint collision. 
 *          
 * 
 * 
 *Checkpoint Saving Collider Behaviour
 * V 1.0
 */

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaveGameManager.Instance.SaveGame(other.transform);
            Debug.Log("Checkpoint reached and game saved.");
        }
    }
}
