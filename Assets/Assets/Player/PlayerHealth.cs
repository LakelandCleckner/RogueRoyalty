using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/* PlayerHealth.cs
 * Nicolas Kaplan (301261925) 
 * 2024-02-25
 * 
 * Last Modified Date: 2024-02-25
 * Last Modified by: Nicolas Kaplan
 * 
 * 
 * Version History:
 *      -> February 21st, 2024
 *          - Created script PlayerHealth.cs to be in charge of all player health functions. 
 *          It also updates the UI for the hearts on-screen.
 *      -> February 25th, 2024
 *          - Added enemy projectile changes, player can now take damage from objects labelled with a "EnemyProj" tag.
 * 
 * 
 * Health for Player
 * V 1.1
 */
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Movement movement;
    [SerializeField] int maxHealth = 3;
    [SerializeField] int playerHealth;

    [SerializeField] Image heart1;
    [SerializeField] Image heart2;
    [SerializeField] Image heart3;

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
            SceneManager.LoadScene("GameOver");
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
        else if (other.gameObject.CompareTag("EnemyProj"))  // enemy projectiles are marked under EnemyProj tag so enemy projectiles don't affect
        {                                                   // other enemies
            LoseHealth(); // if player does not lose health it is because the projectile is destroyed before it can damage them
        }
        if (other.gameObject.CompareTag("HealthPickup"))
        {
            Destroy(other.gameObject);
            GainHealth();
            // play healing sound effect(s)
            

        }
    }
}
