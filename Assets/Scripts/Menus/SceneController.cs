using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * Source File Name: SceneController.cs
 * Author Name: Alexander Maynard
 * Student Number: 301170707
 * Creation Date: January 30th, 2024
 * 
 * Last Modified by: Alexander Maynard
 * Last Modified Date: April 12th, 2024
 * 
 * 
 * Program Description: 
 *      
 *      This script handles the Game Over, 
 *      Main Menu, Options and other various Menu Buttons functionality 
 *      throughout the game.
 *   
 *   
 * Revision History:
 *      -> January 30th, 2024:
 *          -Created this script and fully implemented it.
 *              
 *      -> January 31st, 2024:
 *          -Added comments and program headers for documentation.
 *          
 *      -> February 1st, 2024:
 *          -Updated program header.
 *          
 *      -> April 12th, 2024
 *          -Added a LoadTutorial button that that lets the player enter the tutorial scene.
 */

/// <summary>
/// This class handles the functionality for the 
/// various button found throughout the game.
/// </summary>
public class SceneController: MonoBehaviour
{
    /// <summary>
    /// ChangeScene calls the proper scene we want by passing the string scene name in the editor.
    /// </summary>
    /// <param name="sceneName">
    /// type string sceneName for the scene we want to change to. 
    /// It is passed through the editor.</param>
    public void ChangeScene(string sceneName)
    {
        //load the scene that we want to call
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Quit game allows the player or developper to quit the game. 
    /// If the game is being played in the editor the isPLaying is set to false.
    /// Otherwise if the game is being played as a build then we call Application.Quit()
    /// to quit the game.
    /// </summary>
    public void QuitGame()
    {
        //checks if in unity editor...
        #if UNITY_EDITOR
            //...if so the exit play
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            //...and if not in editor then quit the application
            Application.Quit();
    }

    /// <summary>
    /// Reloads the current scene. NOTE: this is only level 1 for now
    /// as there will only be one level for our game.
    /// </summary>
    public void PlayAgain()
    {
        //Loads the first level.
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// Loads the tutoriaal scene.
    /// </summary>
    public void LoadTutorial()
    {
        //Loads the first level.
        SceneManager.LoadScene("Tutorial");
    }
}