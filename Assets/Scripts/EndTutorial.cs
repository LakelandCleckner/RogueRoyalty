/* EndTutorial.cs
 * 
 * Last Modified Date: 2024-04-15
 * Last Modified by: Alexander Maynard
 * 
 * Version History:
 * 
 *      -> ....
 * 
 *      -> April 15th, 2024 (by Alexander Maynard):
 *          - Rerouted this script to go to the main menu.
 *          - Rerouted this script to go to back to Level 1
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTutorial : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            SceneManager.LoadScene("Level1");
            QuestManager.Instance.CompleteTutorial();
            QuestManager.Instance.CompleteQuest("Tutorial");
        }
    }
}