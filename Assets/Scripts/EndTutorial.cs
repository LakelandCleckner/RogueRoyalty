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
        }
    }
}
