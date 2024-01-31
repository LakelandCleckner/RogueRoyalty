/*
 * Source File Name: PauseMenuController.cs
 * Author Name: Alexander Maynard
 * Student Number: 301170707
 * Last Modified Date: January 31st, 2024
 * 
 * Program Description: 
 *      
 *      This script handles the In-Game pause menu by searching for user input
 *      on the ESC key to bring up the pause menu. The 'Resume' button on the 
 *      menu de-activates the menu and resumes the game. This script also sets the timescale 
 *      to 0 (paused) or 1 (normal) depending if the menu is called or not. 
 *      In addition to this, the Menu button functionality is held here (Resume and Main Menu). 
 * 
 * 
 * Revision History:
 *      -> January 31st, 2024:
 *              -Created this script and fully implemented it.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles the In-Game pause menu by seraching for user input
/// on the ESC key to bring up the pause menu. The 'Resume' button on the 
/// menu de-activates the menu and resumes the game. This script also sets the timescale 
/// to 0 (paused) or 1 (normal) depending if the menu is called or not. 
/// In addition to this, the Menu button functionality is held here (Resume and Main Menu). 
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    //Pause Menu Canvas Objecy
    [Header("Pause Menu Canvas Object")]
    [SerializeField] private GameObject _pauseMenu;

    /// <summary>
    /// This just makes sure that the 
    /// Pause Menu is de-activated by default.
    /// </summary>
    private void Start()
    {
        _pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Update searches for user input on the ESC key. 
    /// ESC key will then activate the In-Game Pause Menu.
    /// </summary>
    void Update()
    {
        //search for user input on the ESC key
        if(Input.GetKey(KeyCode.Escape))
        {
            //set timeScale in the game to pause the scene.
            Time.timeScale = 0.0f;
            //set the pause menu canvas object as activated
            _pauseMenu.SetActive(true);
        }
    }

    /// <summary>
    /// resume the current scene
    /// </summary>
    public void ResumeGame()
    {
        //When the resume button is pressed, put the timeScale back to one...
        Time.timeScale = 1.0f;
        //...and set the set the pause menu canvas object as de-activated.
        _pauseMenu.SetActive(false);
    }

    /// <summary>
    /// load main menu
    /// </summary>
    public void MainMenu()
    {
        //Load the main menu scene (buildIndex 0)
        SceneManager.LoadScene(0);
    }
}
