/*
 * Source File Name: PersistSingleton.cs
 * Author Name: Alexander Maynard
 * Student Number: 301170707
 * Creation Date: March 28th, 2024
 * 
 * Last Modified by: Alexander Maynard
 * Last Modified Date: March 28th, 2024
 * 
 * **NOTE: This script was made by using the PersistentSingleton.cs script from the COMP397 in-class lab project as a REFERENCE**
 * 
 * 
 * Program Description: 
 *      
 *      This script enforces the persistent singleton pattern for gameobjects in a more simple way.
 * 
 * 
 * Revision History:
 *      -> March 28th, 2024
 *          -Created this script and fully implemented it.
 *          -Added the proper documentation.
 */
using UnityEngine;


/// <summary>
/// This class enforces the singleton pattern.
/// </summary>
/// <typeparam name="T">Generic type for any type of object wanting to enforce the singleton.</typeparam>
public abstract class PersistSingleton<T> : MonoBehaviour where T : Component
{
    //public instance with private set
    [HideInInspector] public static T Instance { get; private set; }


    //on awake validate so that only one instance is in the game at all times.
    protected virtual void Awake()
    {
        //if there is no instance or the instance is not of this type...
        if (Instance != null && Instance != this)
        {
            //destroy the gameobject
            Destroy(this.gameObject);
            return;
        }

        //if there is no instance of this object...
        if (Instance == null)
        {
            //create a new instance
            Instance = this as T;
            //make sure it is not destroyed
            DontDestroyOnLoad(Instance);
        }
    }
}
