/* DodgeCheck.cs
 * 
 * Author: Alexander Maynard
 * Creation Date: 2024-04-15
 * 
 * Last Modified Date: 2024-04-15
 * Last Modified by: Alexander Maynard
 * 
 * 
 * Program Description: This script checks if the player has dodged the troll. If so it'll complete the 'Dodge Troll' quest
 * 
 * Version History:
 * 
 * 
 *      -> April 15th, 2024 (by Alexander Maynard):
 *          - Refactored the script to call the WinScreen scene and set the Reach The Goal Quest
 *          -Removed the needed reference of the player and gameover screen.
 */

using UnityEngine;

public class DodgeCheck : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            QuestManager.Instance.CompleteQuest("Dodge Troll");
        }
    }
}
