using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public Text questText;
    public GameObject winScreen;

    private List<Quest> quests = new List<Quest>();

    // Initialize quests
    void Start()
    {
        // Assuming you have two quests: "Kill Enemy" and "Dodge Troll"
        quests.Add(new Quest("Kill Enemy", false));
        quests.Add(new Quest("Dodge Troll", false));
        quests.Add(new Quest("Reach The Goal", false));

        UpdateQuestUI();
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