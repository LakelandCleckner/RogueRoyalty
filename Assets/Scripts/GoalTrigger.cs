/* GoalTrigger.cs
 * 
 * Last Modified Date: 2024-04-15
 * Last Modified by: Alexander Maynard
 * 
 * Version History:
 * 
 *      -> ....
 * 
 *      -> April 15th, 2024 (by Alexander Maynard):
 *          - Refactored the script to call the WinScreen scene and set the Reach The Goal Quest
 *          -Removed the needed reference of the player and gameover screen.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //set the achievement and call the winscreen
            QuestManager.Instance.CompleteQuest("Reach The Goal");
            SceneManager.LoadScene("WinScreen");
        }
    }
}