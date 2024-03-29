/*
 * Source File Name: FireballMover.cs
 * Author Name: Alexander Maynard
 * Student Number: 301170707
 * Creation Date: February 24th, 2024
 * Last Modified by: Nicolas Kaplan
 * Last Modified Date: February 25th, 2024
 * 
 * 
 * Program Description: This script handles the movement and destruction of the fireball gameobject.
 *      
 * 
 * 
 * Revision History:
 *      -> February 24th, 2024:
 *          -Added initial functionality for moving towards the player and fireball destruction
 *      -> February 25th, 2024:
 *          - Made fireball deal damage to Player
 *      -> March 28th, 2024:
 *          -Refactored this script to include the Object Pooling Pattern.
 *          
 */
using UnityEngine;

public class FireballMover : MonoBehaviour
{
    private Transform _playerTransform;
    private Rigidbody _fireBallRigidbody;

    /// <summary>
    /// Finds the player position and adds force to accelerate the fireball towards the player.
    /// </summary>
    void Start()
    {
        //get the player transform and fireball rigidbody
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _fireBallRigidbody = GetComponent<Rigidbody>();

        //this addforce towards the player (player transform - fireball transfrom gets the position to shoot at)
        _fireBallRigidbody.AddForce(_playerTransform.position - this.transform.position, ForceMode.Impulse);
    }

    //when the fireball hits something...
    private void OnCollisionEnter(Collision other)
    {
        //.... then destroy the fireball
        if (other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            //Debug.Log($"fireball found {other.gameObject.name}"); --> Don't need to see this now.
            other.gameObject.GetComponent<PlayerHealth>().LoseHealth();
        }
        FireBallPoolManager.Instance.ReturnPrefabToPool(this);
    }
}
