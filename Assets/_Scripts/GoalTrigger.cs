using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public GameObject gameOverScreen; // Reference to the game over screen

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            // Show the game over screen
            gameOverScreen.SetActive(true);

            // Optionally, pause the game or perform other actions
            Time.timeScale = 0f; // This pauses the game
        }
    }
}