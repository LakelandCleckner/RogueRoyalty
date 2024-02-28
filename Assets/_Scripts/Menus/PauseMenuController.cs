/*
 * Source File Name: PauseMenuController.cs
 * Author Name: Alexander Maynard
 * Student Number: 301170707
 * Creation Date: January 31st, 2024
 * 
 * Last Modified by: Alexander Maynard
 * Last Modified Date: January 31st, 2024
 * 
 * 
 * Program Description: 
 *      
 *      This script handles the In-Game pause menu by searching for user input
 *      on the ESC key to bring up the pause menu. It also disables the InputManager on the player.
 *      The 'Resume' button on the menu de-activates the menu, resumes the game and player InputManager. 
 *      This script also sets the timescale to 0 (paused) or 1 (normal) depending if the menu is called or not. 
 *      In addition to this, the Menu button functionality is held here (Resume and Main Menu). 
 * 
 * 
 * Revision History:
 *      -> January 31st, 2024:
 *          -Created this script and fully implemented it.
 *      -> February 1st, 2024:
 *          -Added OnAwake to find the Player's InputManager so it can be disabled when the pause menu is activated.
 *          -Added switching between cursor lock modes.
 *          -Refactored code and comments to better ecapsulate and control the flow of the game pausing/un-pausing.
 *     -> February 28th, 2024:
 *          -Fixed pause menu
 */

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles the In-Game pause menu by seraching for user input
/// on the ESC key to bring up the pause menu. When the game is paused, so is the Player's InputManager script.
/// The 'Resume' button on the menu de-activates the menu and resumes the game. This script also sets the timescale 
/// to 0 (paused) or 1 (normal) depending if the menu is called or not. 
/// In addition to this, the Menu button functionality is held here (Resume and Main Menu). 
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    //Pause Menu Canvas Objecy
    [Header("Pause Menu Canvas Object")]
    [SerializeField] private GameObject _pauseMenu;


    //Player input object. This will be used to turn-off player input
    [SerializeField] private InputManager _inputManager;

    /// <summary>
    /// Finds the InputManager script on the player 
    /// so it ca be disabled later.
    /// </summary>

    /// <summary>
    /// This just makes sure that the 
    /// Pause Menu is de-activated by default.
    /// </summary>
    void Start()
    {
        //make sure the pause menu is off at the start
        _pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Update searches for user input on the ESC key. 
    /// ESC key will then activate the In-Game Pause Menu
    /// and disable the player input and set the cursor lock mode to none.
    /// </summary>
    void Update()
    {
        //search for user input on the ESC key
        if(Input.GetKey(KeyCode.Escape))
        {
            //calls CanPauseGame
            CanPauseGame(true);
        }
    }

    /// <summary>
    /// Resumes or pauses the game bases on the  bool passed to this function.
    /// </summary>
    /// <param name="isPaused">True means game is pause, false means game is resumed.</param>
    void CanPauseGame(bool isPaused)
    {
        //checks if the game is paused
        switch (isPaused)
        {
            //true so....
            //...set timeScale in the game to pause the scene,
            //set the pause menu canvas object as activated,
            //disable the player input and unlock the cursor.
            case true:
                Time.timeScale = 0.0f;
                _pauseMenu.SetActive(true);
                _inputManager.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                break;
            //false so...
            //...set timeScale to 1 to resume the game,
            //set the set the pause menu canvas object as de-activated,
            //re-enable player input and lock the cursor for gameplay again
            case false:
                Time.timeScale = 1.0f;
                _pauseMenu.SetActive(false);
                _inputManager.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }


    //------------BUTTONS------------------------//

    /// <summary>
    /// Resume game fuctionality. Calls the private method for the actual functionality.
    /// </summary>
    public void ResumeGame()
    {
        CanPauseGame(false);
    }

    /// <summary>
    /// load main menu
    /// </summary>
    public void MainMenu()
    {
        //Load the main menu scene (buildIndex 0)
        SceneManager.LoadScene(0);
    }

    //------------END OF BUTTONS------------------------//
}
