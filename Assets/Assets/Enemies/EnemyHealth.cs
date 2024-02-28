using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/* EnemyHealth.cs
 * Nicolas Kaplan (301261925) 
 * 2024-02-25
 * 
 * Last Modified Date: 2024-02-25
 * Last Modified by: Nicolas Kaplan
 * 
 * 
 * Version History:
 *      -> February 25th, 2024
 *          - Created script EnemyHealth.cs to be in charge of all enemy health functions. 
 *          
 *          
 * 
 * 
 * Health for Enemy
 * V 1.0
 */
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] int enemyHealth;


    private void Start()
    {
        enemyHealth = maxHealth;
        gameObject.GetComponent<Movement>();
    }

    public void LoseHealth()
    {
        if (enemyHealth > 1)
        {
            enemyHealth--;
            Debug.Log($"{gameObject.name} lost health.");

        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void GainHealth()
    {
        if (enemyHealth < maxHealth) 
        {
            enemyHealth++;
            Debug.Log($"{gameObject.name} gained health.");

        }

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathZone"))
        {
            // destroy enemy if it enters a death zone (clean up)
            Destroy(gameObject);

        }
        else if (other.gameObject.CompareTag("Projectile")) // projectile is an undefined tag at the moment, it will be implemented with crossbow
        {
            LoseHealth();
        }
        if (other.gameObject.CompareTag("HealthPickup"))
        {
            Destroy(other.gameObject);
            GainHealth();
            // play healing sound effect(s)
        }
    }
}
