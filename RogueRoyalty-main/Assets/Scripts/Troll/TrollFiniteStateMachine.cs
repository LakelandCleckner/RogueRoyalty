/*
 * Source File Name: SkeletonFiniteStateMachine.cs
 * Author Name: Alexander Maynard
 * Student Number: 301170707
 * Creation Date: February 26th, 2024
 * 
 * Last Modified by: Alexander Maynard
 * Last Modified Date: February 27th, 2024
 * 
 * 
 * Program Description: This script handles the behaviours of the Troll enemy type. Such states include: Guard, Approach Stunned and Attack.
 *      
 * Revision History:
 *      -> February 26th, 2024:
 *          -Added boilerplate layout for the troll FSM with implementations for the approach, stunned and guard states. 
 *           The Attack state and comments are not complete.
 *          -Added some comments.
 *          
 *      -> February 27th, 2024:
 *          -Finished all state functionality and wired up the animator.
 *          -Completed comments
 */

using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The purpose of this class is the define the behaviour of the Troll and all the flow between all its behavioral states.
/// </summary>
public class TrollFiniteStateMachine : MonoBehaviour
{
    // All Troll states
    public enum TrollState { Guard, Approach, Stunned, Attack }

    // Current troll state
    [Header("Current Troll state")]
    [SerializeField] private TrollState _currentTrollState;

    // Awarness variables (in the nav mesh agant component)
    [Header("Troll awareness and attack radius variables")]
    [SerializeField] private float _awarenessRadius = 5.0f;
    [SerializeField] private float _attackRadius = 2.0f;

    // Speed variables (in the nav mesh agant component)
    [Header("Troll approaching speed variables")]
    [SerializeField] private float _approachSpeed = 2.0f;
    [SerializeField] private float _returnSpeed = 1.0f;

    // Reference to the player
    [Header("Player Transform reference")]
    private Transform _player;

    // Nav mesh related variables
    [Header("Nav Mesh related variables for AI pathfinding")]
    private NavMeshAgent _agent;

    [Header("Guard point variables")]
    [SerializeField] private Transform _guardPoint;
    [SerializeField] private Transform _guardLookPoint;

    // Animator for the Troll
    [Header("Animator for Troll")]
    private Animator _animator;

    // Check for if the troll is stunned
    [Header("Check if Troll is stunned")]
    [SerializeField] private bool _isStunned = false;

    // Reference to the axe collider
    [Header("Reference to the troll axe collider")]
    [SerializeField] private GameObject _axeCollider;

    // Sound for walking
    [Header("Audio")]
    [SerializeField] private AudioClip _walkingSound;
    private AudioSource _audioSource;
    public AudioSource otherAudioSource; // Reference to the external AudioSource
    public AudioClip attackSound; // Define the attack sound

    /// <summary>
    /// Assigns all needed references to the proper variables.
    /// </summary>
    void Awake()
    {
        _currentTrollState = TrollState.Guard;
        _agent = GetComponent<NavMeshAgent>();

        // Find the player
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        // Get the animator for the troll
        _animator = GetComponent<Animator>();
        // Get the audio source
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Calls the TrollFSM
    /// </summary>
    void FixedUpdate()
    {
        TrollFSM();
    }

    /// <summary>
    /// Function calls a switch statement to help control the flow bewtween all Troll states
    /// </summary>
    void TrollFSM()
    {
        switch (_currentTrollState)
        {
            case TrollState.Guard:
                HandleGuard();
                break;
            case TrollState.Approach:
                HandleApproach();
                break;
            case TrollState.Stunned:
                HandleStunned();
                break;
            case TrollState.Attack:
                HandleAttack();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Method changes the troll state with the new one provided
    /// </summary>
    /// <param name="newState">This parameter is the new state wanting to be called</param>
    void ChangeState(TrollState newState) => _currentTrollState = newState;

    // Handlers------------------------------

    /// <summary>
    /// Handles the flow of the Guard state and the transition to other states
    /// </summary>
    void HandleGuard()
    {
        // If stunned change state to stunned state --> done first as any state can call stunned
        if (_isStunned)
        {
            ChangeState(TrollState.Stunned);
        }
        // Else if can't sense enemy change state to guard state
        else if (!SenseEnemy())
        {
            // If no player guard
            Guard();
        }
        // Else if can sense enemy change state to approach state
        else if (SenseEnemy())
        {
            // Then approach the player
            ChangeState(TrollState.Approach);
        }
    }

    /// <summary>
    /// Handles the flow of the Approach state and the transition to other states
    /// </summary>
    void HandleApproach()
    {
        // If stunned change state to stunned state --> done first as any state can call stunned
        if (_isStunned)
        {
            ChangeState(TrollState.Stunned);
        }
        // Else if can sense enemy but can't attack change state to approach state
        else if (SenseEnemy() && !CanAttack())
        {
            Approach();
        }
        // Else if can't sense enemy and can't attack, change state to guard state
        else if (!SenseEnemy() && !CanAttack())
        {
            ChangeState(TrollState.Guard);
        }
        // Else if can sense enemy and can attack change state to attack state
        else if (CanAttack() && SenseEnemy())
        {
            ChangeState(TrollState.Attack);
        }
    }

    /// <summary>
    /// Handles the flow of the Stunned state and the transition to other states
    /// </summary>
    void HandleStunned()
    {
        // If stunned change state to stunned state --> done first as any state can call stunned
        if (_isStunned)
        {
            // If the Troll is stunned set _isStunned to false so Stunned is only called once
            _isStunned = false;
            Stunned();
        }
        // Else if can sense enemy but can't attack, change state to approach state
        else if (SenseEnemy() && !CanAttack())
        {
            ChangeState(TrollState.Approach);
        }
        // Else if can't sense and can't attack enemy change state to guard state
        else if (!SenseEnemy() && !CanAttack())
        {
            ChangeState(TrollState.Guard);
        }
        // Else if can sense enemy and can attack, change state to attack state
        else if (CanAttack() && SenseEnemy())
        {
            ChangeState(TrollState.Attack);
        }
    }

    /// <summary>
    /// Handles the flow of the Attack state and the transition to other states
    /// </summary>
    void HandleAttack()
    {
        // If stunned change state to stunned state --> done first as any state can call stunned
        if (_isStunned)
        {
            ChangeState(TrollState.Stunned);
        }
        // Else if can sense enemy and can attack, change state to attack state
        else if (CanAttack() && SenseEnemy())
        {
            Attack();
            // Play attack sound
            if (_audioSource != null && attackSound != null)
            {
                _audioSource.clip = attackSound;
                _audioSource.Play();
            }
        }
        // Else if can sense enemy but can't attack, change state to approach state
        else if (!CanAttack() && SenseEnemy())
        {
            ChangeState(TrollState.Approach);
        }
    }

    // End of handlers-----------------------

    /// <summary>
    /// The functionality implementations for the states are called by their respective handlers.
    /// When called they start the functionality for their respective states.
    /// </summary>

    // Functionality-------------------------

    /// <summary>
    /// This is the Guard state implementation for the Troll.
    /// </summary>
    void Guard()
    {
        // Set the return speed
        _agent.speed = _returnSpeed;
        // Set the agent to go back to the guard point with AI pathfinding
        _agent.destination = _guardPoint.position;
        // Make sure the attack animation bool is false
        _animator.SetBool("canAttack", false);
        // Make sure the axe collider is off
        _axeCollider.SetActive(false);

        // Check if the Troll is at the guard point...
        if (Vector3.Distance(_agent.transform.position, _guardPoint.transform.position) < 0.2f)
        {
            // If so, set the needToMove animation bool off
            _animator.SetBool("needToMove", false);
            // Make sure the agent is looking at the guardLookPoint when he is stationed properly
            _agent.transform.LookAt(_guardLookPoint.transform.position);
        }
        ///...otherwise the troll is not back at his guard point so...
        else
        {
            // Make sure the needToMove animation bool is still true to display the running animation.
            _animator.SetBool("needToMove", true);
            // Play walking sound
            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = _walkingSound;
                _audioSource.Play();
            }
        }
    }

    /// <summary>
    /// This is the Approach state implementation for the Troll.
    /// </summary>
    void Approach()
    {
        // Set the speed to approach the player
        _agent.speed = _approachSpeed;
        // Set the agent destination as the player for AI pathfinding
        _agent.destination = _player.transform.position;
        // Make sure the axecollider that can damage the player is off still
        _axeCollider.SetActive(false);

        // Set the needToMove bool as true and the canAttack
        // bool is false to keep playing the run animation for the troll
        _animator.SetBool("needToMove", true);
        _animator.SetBool("canAttack", false);
        // Play walking sound
        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = _walkingSound;
            _audioSource.Play();
        }
    }

    /// <summary>
    /// This is the Stunned state implementation for the Troll.
    /// </summary>
    void Stunned()
    {
        // Briefly set the destination to the current position to 'stop' the troll briefly
        _agent.destination = _agent.transform.position;
        // Make sure the axecollider that can damage the player is off still
        _axeCollider.SetActive(false);

        // Set the isStunned trigger to play the troll 'getting hit' animation
        _animator.SetTrigger("isStunned");
    }

    /// <summary>
    /// This is the Attack state implementation for the Troll.
    /// </summary>
    void Attack()
    {
        // Set the destination to the current position to 'stop' the troll
        _agent.destination = _agent.transform.position;
        // Make the AI pathfinding agent look at the player
        _agent.transform.LookAt(_player.transform.position);

        // Set the axe collider to hit the player to active or on
        _axeCollider.SetActive(true);

        // Set the canAttack animation bool to true and the needToMove
        // animation to false to display the troll attack animation
        _animator.SetBool("canAttack", true);
        _animator.SetBool("needToMove", false);
    }

    // End of functionality------------------

    /// <summary>
    /// The functionality checks provide the a way to examine whether the states can transition.
    /// </summary> 

    // Functionality checks------------------

    /// <summary>
    /// Checks if the troll can sense the player by calculating the distance between the itself and player,
    /// and assessing if the player is close enough to be seen
    /// </summary>
    /// <returns> true or false if the enemy can sense the player</returns>
    bool SenseEnemy()
    {
        // If distance between player and enemy is below the awareness radius for the player then enemy can sense the player
        // so return true...
        if (Vector3.Distance(_player.position, _agent.transform.position) < _awarenessRadius) return true;
        // ...otherwise return false
        else return false;
    }

    /// <summary>
    /// Checks if the troll can attack the player by calculating the distance between the itself and player,
    /// and assessing if the player is close enough to be attacked
    /// </summary>
    /// <returns> true or false if the enemy can attack the player</returns>
    bool CanAttack()
    {
        // If distance between player and enemy is below the attack radius for the player then enemy can attack the player
        // so return true...
        if (Vector3.Distance(_player.position, _agent.transform.position) < _attackRadius) return true;
        // ...otherwise return false
        else return false;
    }

    // Checks if the troll was hit by the player projectile, to tell the troll FSM that the troll is stunned
    private void OnTriggerEnter(Collider other)
    {
        // If the troll was hit by the Projectile tag... 
        if (other.gameObject.CompareTag("Projectile"))
        {
            // ...then set isStunned to true to make sure the Troll FSM updates accordingly
            _isStunned = true;
        }
    }

    // End of functionality checks-----------

    /// <summary>
    /// OnDrawGizmos draws the awareness radius and attack radius for the Troll in the editor for testing. 
    /// </summary>
    // OnDrawGizmos--------------------------
    // Visual lines in editor to see the sense radius for testing the enemy
    private void OnDrawGizmos()
    {
        // Set gizmo color
        Gizmos.color = Color.yellow;
        // Shows troll attack radius radius
        Gizmos.DrawWireSphere(this.transform.position, _attackRadius);

        // Set gizmo color
        Gizmos.color = Color.blue;
        // Shows troll detection radius radius
        Gizmos.DrawWireSphere(this.transform.position, _awarenessRadius);
    }
    // End of OnDrawGizmos-------------------
}
