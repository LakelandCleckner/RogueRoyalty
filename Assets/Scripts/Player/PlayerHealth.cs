using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/* PlayerHealth.cs
 * Nicolas Kaplan (301261925) 
 * 2024-02-25
 * 
 * Last Modified Date: 2024-02-25
 * Last Modified by: Alexander Maynard
 * 
 * 
 * Version History:
 *      -> February 21st, 2024
 *          - Created script PlayerHealth.cs to be in charge of all player health functions. 
 *          It also updates the UI for the hearts on-screen.
 *          
 *      -> February 25th, 2024
 *          - Added enemy projectile changes
 *          
 *      -> April 15th, 2024 (by Alexander Maynard):
 *          - Added scene checks to lead the proper game over scene for either the tutorial or regualer level 1
 * 
 * 
 * Health for Player
 * V 1.2
 */
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] int maxHealth = 3;
    [SerializeField] int playerHealth;

    [SerializeField] Image heart1;
    [SerializeField] Image heart2;
    [SerializeField] Image heart3;

    [SerializeField] AchievementManager achievements;
    private void Start()
    {
        playerHealth = maxHealth;
        UpdateHeartUI();
        gameObject.GetComponent<Movement>();
    }

    public void LoseHealth()
    {
        if (playerHealth > 1)
        {
            playerHealth--;
            Debug.Log("Player lost health.");
            UpdateHeartUI();

        }
        else
        {
            //check if we are in Level1 
            if(SceneManager.GetActiveScene().name == SceneManager.GetSceneByName("Level1").name)
            {
                //if so then load the regular game over
                SceneManager.LoadScene("GameOver");
                achievements.GiveAchievement(1);
            }
            //check if we are in the tutorial scene
            else if (SceneManager.GetActiveScene().name == SceneManager.GetSceneByName("Tutorial").name)
            {
                //if so then load the tutorial version of the game over
                SceneManager.LoadScene("GameOverTutorial");
            }
        }
    }
    public void GainHealth()
    {
        if (playerHealth < maxHealth) 
        { 
            playerHealth++;
            UpdateHeartUI();
            Debug.Log("Player gained health.");

        }

    }
    void UpdateHeartUI()
    {
        if (playerHealth == 3)
        {
            heart1.enabled = true;
            heart2.enabled = true;
            heart3.enabled = true;
        }
        else if (playerHealth == 2)
        {
            heart1.enabled = true;
            heart2.enabled = true;
            heart3.enabled = false;
        }
        else if (playerHealth == 1)
        {
            heart1.enabled = true;
            heart2.enabled = false;
            heart3.enabled = false;
        }
        else
        {
            heart1.enabled = false;
            heart2.enabled = false;
            heart3.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
        {
            LoseHealth();
            // play damage sound effect(s)
            movement.SendToCheckpoint(); // in Movement.cs there's a method called SendToCheckpoint() which is called here to save on time.
        }
        
        if (other.gameObject.CompareTag("HealthPickup"))
        {
            Destroy(other.gameObject);
            GainHealth();
            // play healing sound effect(s)
        }
    }
}
