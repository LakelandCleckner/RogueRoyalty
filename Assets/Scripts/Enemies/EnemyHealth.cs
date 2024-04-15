using UnityEngine;

/* EnemyHealth.cs
 * Nicolas Kaplan (301261925) 
 * 2024-02-25
 * 
 * Last Modified Date: April 15th, 2024
 * Last Modified by: Alexander Maynard
 * 
 * 
 * Version History:
 *      -> February 25th, 2024
 *          - Created script EnemyHealth.cs to be in charge of all enemy health functions. 
*       -> February 28th, 2024
 *          - Made int enemyHealth public so that it can be accessed from MouseLook.cs. 
 *      -> March 15th, 2024 (by Alexander Maynard):
 *          - Refactored code to make the Raycast decrement health.
 *      -> April 10th, 2024
 *          - Made AchievementManager achievements; and called it so it can be used to 
 *          send an achievement to the player when they kill their first enemy.  
 *      -> April 15th, 2024 (by Alexander Maynard):
 *          -Added a Quest for killing an enemy here.
 * 
 * 
 * Health for Enemy
 * V 1.2
 */
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] public int enemyHealth;
    [SerializeField] private GameObject _thingToDestroy;

    [SerializeField] AchievementManager achievements;

    private void Start()
    {
        enemyHealth = maxHealth;
        gameObject.GetComponent<Movement>();
    }

    private void Update()
    {
        if(enemyHealth <= 0)
        {
            Destroy(_thingToDestroy);
            achievements.GiveAchievement(0);
            QuestManager.Instance.CompleteQuest("Kill Enemy");
        }
    }

    public void LoseHealth()
    {
        enemyHealth--;
    }
    public void GainHealth()
    {
        if (enemyHealth < maxHealth) 
        {
            enemyHealth++;
            Debug.Log($"{gameObject.name} gained health.");

        }

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
        {
            // destroy enemy if it enters a death zone (clean up)
            Destroy(gameObject);

        }
        else if (other.gameObject.CompareTag("Projectile")) // projectile is an undefined tag at the moment, it will be implemented with crossbow
        {
            LoseHealth();
        }
        if (other.gameObject.CompareTag("HealthPickup"))
        {
            Destroy(other.gameObject);
            GainHealth();
            // play healing sound effect(s)
        }
    }
}
