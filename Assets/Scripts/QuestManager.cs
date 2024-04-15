/* QuestManager.cs
 * 
 * Last Modified Date: 2024-04-15
 * Last Modified by: Alexander Maynard
 * 
 * Version History:
 * 
 *      -> ....
 * 
 *      -> April 15th, 2024 (by Alexander Maynard):
 *          - Refactored the Quest Sytem to implement the persistent singleton instead of it's own singleton logic, so it could persist across scenes.
 *          - Added Tutorial Quest
 *          - Added logic for the Quest sytem UI to disable when in scenes it's not needed.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class QuestManager : PersistSingleton<QuestManager>
{
    public bool IsTutorialCompleted { get; private set; }
    public TextMeshProUGUI questText;
    public GameObject winScreen;

    private List<Quest> quests = new List<Quest>();

    [SerializeField] private GameObject _questPanel;

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

    // Initialize quests
    void Start()
    {
        winScreen = GameObject.FindGameObjectWithTag("WinScreen");
        // Assuming you have two quests: "Kill Enemy" and "Dodge Troll"
        quests.Add(new Quest("Kill Enemy", false));
        quests.Add(new Quest("Dodge Troll", false));
        quests.Add(new Quest("Reach The Goal", false));
        quests.Add(new Quest("Tutorial", false));

        UpdateQuestUI();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == SceneManager.GetSceneByName("Tutorial").name ||
            SceneManager.GetActiveScene().name == SceneManager.GetSceneByName("Level1").name)
        {
            _questPanel.SetActive(true);
        }
        else
        {
            _questPanel.SetActive(false);
        }
    }

    // Update quest UI
    void UpdateQuestUI()
    {
        string questInfo = "Quests:\n";
        foreach (Quest quest in quests)
        {
            questInfo += quest.name + " - " + (quest.completed ? "Complete\n" : "Incomplete\n");
        }
        questText.text = questInfo;
    }

    // Mark quest as complete
    public void CompleteQuest(string questName)
    {
        Quest quest = quests.Find(q => q.name == questName);
        if (quest != null)
        {
            quest.completed = true;
            UpdateQuestUI();
            CheckWinCondition();
        }
    }

    // Check win condition
    void CheckWinCondition()
    {
        bool allQuestsCompleted = true;
        foreach (Quest quest in quests)
        {
            if (!quest.completed)
            {
                allQuestsCompleted = false;
                break;
            }
        }

        if (allQuestsCompleted)
        {
            WinGame();
        }
    }

    // Display win screen
    void WinGame()
    {
        winScreen.SetActive(true);
    }
}

public class Quest
{
    public string name;
    public bool completed;

    public Quest(string _name, bool _completed)
    {
        name = _name;
        completed = _completed;
    }
}