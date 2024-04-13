using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Source File Name: UICursorHandler.cs
 * Author Name: Alexander Maynard
 * Student Number: 301170707
 * Creation Date: February 4th, 2024
 * 
 * Last Modified by: Alexander Maynard
 * Last Modified Date: February 4th, 2024
 * 
 * 
 * Program Description: 
 *      
 *      This script makes sure that the cursor lock mode for the menus is set to none.
 * 
 * Revision History:
 *      -> February 4th, 2024:
 *          -Created this script and fully implemented it.g.
 */


/// <summary>
/// Class sets cursor lock mode for the menus.
/// </summary>
public class UICursorHandler : MonoBehaviour
{
    /// <summary>
    /// Sets cursor lock mode
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
