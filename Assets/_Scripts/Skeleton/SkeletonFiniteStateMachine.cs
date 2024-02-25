/*
 * Source File Name: SkeletonFiniteStateMachine.cs
 * Author Name: Alexander Maynard
 * Student Number: 301170707
 * Creation Date: February 25th, 2024
 * Last Modified by: Alexander Maynard
 * Last Modified Date: February 25th, 2024
 * 
 * 
 * Program Description: This script handles the behaviours of the Skeleton enemy type. Such states include: Patrol, Chase and 'Explode'.
 *      
 * Revision History:
 *      -> February 25th, 2024:
 *          -Added all states and functionality for the Skeleton FSM
 *          -Added all comments and comment headers
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The purpose of this class is the define the behaviour of the Skeleton all the flow between all its behavioural states.
/// </summary>
public class SkeletonFiniteStateMachine : MonoBehaviour
{

    //magmaMonsterStates
    public enum SkeletonState { Patrol, Chase, Explode }

    //current monster state
    [Header("Current Skeleton state")]
    [SerializeField] private SkeletonState _currentSkeletonState;

    //awarness variables (in the nav mesh agant component)
    [Header("Skeleton Awareness Variable")]
    [SerializeField] private float _awarenessRadius = 20.0f;

    //speed variables (in the nav mesh agant component)
    [Header("Skeleton Awareness Variable")]
    [SerializeField] private float _patrolSpeed = 5.0f;
    [SerializeField] private float _chaseSpeed = 10.0f;



    //patrol path navigation points for the enemy
    [Header("Skeleton Patrol Points")]
    [SerializeField] private int _currentPosition = 0;
    [SerializeField] private List<Transform> _patrolPositions;

    //reference to the player
    [Header("Player Transform reference")]
    private Transform _player;

    //Nav mesh related variables
    [Header("Nav Mesh related variables for AI pathfinding")]
    private Vector3 _destination;
    private NavMeshAgent _agent;


    //Animator for the Skeleton
    [Header("Animator for Skeleton")]
    private Animator _animator;

    //check for if the skeleton can explode
    [Header("Check if Skeleton can explode")]
    [SerializeField] private bool _canExplode = false;

    //bomb reference added in the editor
    [Header("Bomb and Explosion references")]
    [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _bombExplosion;

    //to ensure that the explosion particles are only called once
    private bool _isBombExploded = false;



    /// <summary>
    /// Assigns all needed references to the proper variables.
    /// </summary>
    void Awake()
    {
        _currentSkeletonState = SkeletonState.Patrol;
        _agent = GetComponent<NavMeshAgent>();
        //sets the current patrol point
        _destination = _patrolPositions[_currentPosition].position;
        _agent.destination = _destination;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Each frame calls the Skeleton Finite State Machine and the timer for the Skeleton attacks
    /// </summary>
    void FixedUpdate()
    {
        SkeletonFSM();
    }

    /// <summary>
    /// Switch statements helps control the flow bewtween all Skeleton states
    /// </summary>
    void SkeletonFSM()
    {
        switch (_currentSkeletonState)
        {
            case SkeletonState.Patrol:
                HandlePatrol();
                break;
            case SkeletonState.Chase:
                HandleChase();
                break;
            case SkeletonState.Explode:
                HandleExplode();
                break;
            default:
                break;
        }
    }

    //method to be called when wanting to change states
    void ChangeState(SkeletonState newState) => _currentSkeletonState = newState;




    /// <summary>
    /// all these handlers help call and control the flow bewtween states
    /// </summary>
    //Handlers------------------------------

    /// <summary>
    /// If the skeleton does not 'sense' the player then continue calling the patrol state to transistion between points.
    /// Else, the enemy senses the player; call the Chase state
    /// </summary>
    void HandlePatrol()
    {
        //check if the skeleton can sense the enemy...
        if (SenseEnemy() == false)
        {
            //call isWalking and disable run anim to change to the walking animation
            _animator.SetBool("isWalking", true);
            _animator.SetBool("isRunning", false);
            //then call patrol
            Patrol();
        }
        //...otherwise
        else
        {
            //call isRunning to true and disable walk to change to the running animation
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isWalking", false);
            //change state the chase state
            ChangeState(SkeletonState.Chase);
        }
    }


    /// <summary>
    /// If the skeleton canexplode call explode state. --> this state takes priority as there is no return from the explosion state.
    /// If the skeleton senses the player, call the Chase state.
    /// Else if, the enemy cant sense the player then call the Patrol state.
    /// </summary>
    void HandleChase()
    {

        //if the skeleton can explode then...
        if (_canExplode == true)
        {
            //call death aka skeleton 'loose' animation
            _animator.SetBool("isExploding", true);
            //then change the state to the explosion state
            ChangeState(SkeletonState.Explode);
        }
        //... otherwise see if the skeleton can sense theh player...
        else if (SenseEnemy() == true)
        {
            //if so then... 
            //call the isRunning animation and disable the walking animation
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isWalking", false);
            //call the chase state
            Chase();
        }
        //otherwise the skeleton can't explode or sense the player...
        else
        {
            //... so call the walking animation and disable the running animation
            _animator.SetBool("isWalking", true);
            _animator.SetBool("isRunning", false);
            //change the state to the patrol state
            ChangeState(SkeletonState.Patrol);
        }
    }

    /// <summary>
    /// If the enemy senses can explode then this handler is called.... there is no return from this state handler.
    /// This handler only calls the explode functionality and has no transitions
    /// </summary>
    void HandleExplode()
    {
        //call Explode()
        Explode();
    }

    //End of handlers-----------------------

    /// <summary>
    /// The functionality implementations for the states are called by their respective handlers.
    /// When called they start the functionality for their respective states.
    /// </summary>

    //Functionality-------------------------

    /// <summary>
    /// Patrol uses the AI pathfinding package to move the enemy between points.
    /// </summary>
    void Patrol()
    {
        //when the enemy gets pretty close to a patrol point (doesn't need to be exact), then can go the next point.
        if (Vector3.Distance(_agent.transform.position, _patrolPositions[_currentPosition].position) < .2f)
        {
            //sets the current point.
            //this helps loop through all the points regardless of how many we have
            _currentPosition = (_currentPosition + 1) % _patrolPositions.Count;
        }
        _agent.speed = _patrolSpeed;
        //let the navmesh agent go to the current point that is assigned
        _agent.destination = _patrolPositions[_currentPosition].position;
    }

    /// <summary>
    /// Chase uses the AI pathfinding package to follow the player
    /// </summary>
    void Chase()
    {
        //set the agent speed to the chase speed
        _agent.speed = _chaseSpeed;
        //set the agent destination to the player position
        _agent.destination = _player.position;
        //assigns the rotation of the _agent to look at the player
        _agent.transform.LookAt(_player.transform.position);
    }


    /// <summary>
    /// Handles functionality for the skeleton explosion (and destruction)
    /// </summary>
    void Explode()
    {
        //stop skeleton movement by stopping the agent.
        _agent.isStopped = true;

        //Instantiate explode particles
        if (_isBombExploded == false)
        {
            //make sure the skelton can't explode again
            _isBombExploded = true;
            //Instantiate explosion particles to the skeleton position
            Instantiate(_bombExplosion, this.transform.position, this.transform.rotation);
        }

        //destroy the bomb attached to the skeleton
        Destroy(_bomb);

        //destroy the skeleton (must destroy the parent object) after 1 second
        Destroy(this.transform.parent.gameObject, 1);
    }

    //End of functionality------------------


    /// <summary>
    /// The functionality checks provide the a way to examine whether the states can transition.
    /// </summary> 
    
    //Functionality checks------------------
    
    /// <summary>
    /// Checks if the enemy can sense the player by calculating the distance between the skeleton and player,
    /// and assessing if the player is close enough to be seen
    /// </summary>
    /// <returns> true or false if the enemy can sense the player</returns>
    bool SenseEnemy()
    {
        //if distance between player and enemy is below the awareness radius for the player then enemy can sense the player
        //so return true...
        if (Vector3.Distance(_player.position, _agent.transform.position) < _awarenessRadius) return true;
        //...otherwise return false
        else return false;
    }


    //checks if the skeleton hit the player, to tell the skeleton FSM that the skeleton can explode
    private void OnTriggerEnter(Collider other)
    {
        //if we hit the player collider... 
        if(other.gameObject.CompareTag("Player"))
        {
            //...then set canExplode to true to let the skeleton FSM know that the skeleton can explodes
            _canExplode = true;
        }
    }

    //End of functionality checks-----------

    /// <summary>
    /// OnDrawGizmos draws the awareness radius for the skeleton in the editor for testing. Fow now, 
    /// it also displays the line between patrol points.
    /// </summary>
    //OnDrawGizmos--------------------------
    //visual lines in editor to see the sense radius for testing the enemy
    private void OnDrawGizmos()
    {
        //displays the lines between patrol points
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_patrolPositions[0].position, _patrolPositions[1].position);
        Gizmos.DrawLine(_patrolPositions[1].position, _patrolPositions[2].position);
        Gizmos.DrawLine(_patrolPositions[2].position, _patrolPositions[3].position);
        Gizmos.DrawLine(_patrolPositions[3].position, _patrolPositions[0].position);

        Gizmos.color = Color.blue;
        //shows player detection radius radius
        Gizmos.DrawWireSphere(this.transform.position, _awarenessRadius);
    }
    //End of OnDrawGizmos-------------------
}
