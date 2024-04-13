/* Movement.cs
 * Nicolas Kaplan (301261925) 
 * 2024-03-31
 * 
 * Last Modified Date: 2024-03-31
 * Last Modified by: Nicolas Kaplan
 * 
 * 
 * Version History:
 *      -> March 31st, 2024
 *          - Created all functionality to make the script move the player with it
 * This script is in charge of player movement with platforms
 * V 1.0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayerMovement : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
