using UnityEngine;

public class SecretAreaAchievementScript : MonoBehaviour
{
    [SerializeField] AchievementManager achievementManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            achievementManager.GiveAchievement(2);
            Destroy(gameObject);
        }
    }
}
