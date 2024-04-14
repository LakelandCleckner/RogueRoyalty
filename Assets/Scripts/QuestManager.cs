using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public bool IsTutorialCompleted { get; private set; }

    public void CompleteTutorial()
    {
        IsTutorialCompleted = true;
        Debug.Log("Tutorial Completed!");
    }
    public void NewgameBtn()
    {
        QuestManager.Instance.CompleteTutorial();
        SceneManager.LoadScene("Level1");
    }
    public int EnemiesKilled { get; private set; }

    public void EnemyKilled()
    {
        EnemiesKilled++;
        Debug.Log("Enemy killed!");
        if (EnemiesKilled >= 1) // Set required kills here
        {
            Debug.Log("Kill Quest Completed!");
            // Handle quest completion
        }
    }
}