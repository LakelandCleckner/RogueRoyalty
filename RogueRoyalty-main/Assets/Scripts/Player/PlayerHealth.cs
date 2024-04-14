using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* PlayerHealth.cs
 * Nicolas Kaplan (301261925) 
 * 2024-02-25
 * 
 * Last Modified Date: 2024-04-13
 * Last Modified by: Nicolas Kaplan
 * 
 * Version History:
 *      -> February 21st, 2024
 *          - Created script PlayerHealth.cs to be in charge of all player health functions. 
 *          It also updates the UI for the hearts on-screen.
 *      -> February 25th, 2024
 *          - Added enemy projectile changes
 *      -> April 13th, 2024
 *          - Added sound effects for taking damage and dying.
 * 
 * Health for Player
 * V 1.3
 */
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int playerHealth;

    [SerializeField] private Image heart1;
    [SerializeField] private Image heart2;
    [SerializeField] private Image heart3;

    [SerializeField] private AchievementManager achievements;

    // Audio sources for damage and death
    [SerializeField] private AudioSource damageSound;
    [SerializeField] private AudioSource deathSound;

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
            damageSound.Play();  // Play damage sound
            UpdateHeartUI();
        }
        else
        {
            deathSound.Play();  // Play death sound before changing scene
            SceneManager.LoadScene("GameOver");
            achievements.GiveAchievement(1);
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
        // Update heart UI based on current health status
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
        else if (playerHealth is 1)
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
            movement.SendToCheckpoint();
        }

        if (other.gameObject.CompareTag("HealthPickup"))
        {
            // Play the sound from the AudioSource attached to the health pickup object
            AudioSource pickupAudio = other.GetComponent<AudioSource>();
            if (pickupAudio != null)
            {
                pickupAudio.Play();
                // Delay the destruction to let the sound play
                Destroy(other.gameObject, pickupAudio.clip.length);
            }
            else
            {
                // Directly destroy the object if no AudioSource is found
                Destroy(other.gameObject);
            }

            GainHealth();
        }
    }
}
