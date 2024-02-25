/*
 * Source File Name: MagmaMonsterFiniteStateMachine.cs
 * Author Name: Alexander Maynard
 * Student Number: 301170707
 * Creation Date: February 24th, 2024
 * Last Modified by: Alexander Maynard
 * Last Modified Date: February 24th, 2024
 * 
 * 
 * Program Description: This script handles the behaviours of the Magma Monster enemy type. Such states include: FlyPath (patrol), Chase and Attack
 *      
 * 
 * 
 * Revision History:
 *      -> February 24th, 2024:
 *          -Added all states and functionality
 *          -Added all comments and comment headers
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The purpose of this class is the define the behaviour of the Magma Monster all the flow between all its behaviiour states.
/// </summary>
public class MagmaMonsterFiniteStateMachine : MonoBehaviour
{
    //magmaMonsterStates
    public enum MagmaMonsterState { FlyPath, Chase, Attack }

    //current monster state
    [Header("Current monster state")]
    [SerializeField] private MagmaMonsterState _currentMonsterState;

    //awarness variables (in the nav mesh agant component)
    [Header("Magma Monster Awareness Variables")]
    [SerializeField] private float _awarenessRadius = 20.0f;
    [SerializeField] private float _attackRadius = 10.0f;

    //flight path navigation points for the enemy
    [Header("Magma Monster Patrol Points")]
    [SerializeField] private int _currentPosition = 0;
    [SerializeField] private List<Transform> _patrolPositions;

    //reference to the player
    [Header("Player Transform reference")]
    [SerializeField] private Transform _player;

    //Nav mesh related variables
    [Header("Nav Mesh related variables for AI pathfinding")]
    [SerializeField] private Vector3 _destination;
    [SerializeField] private NavMeshAgent _agent;


    //Animator for the MagmaMonster
    [Header("Animator for the Magma Monster")]
    [SerializeField] private Animator _animator;

    //fireball to be shot at player
    [Header("Fireball related references")]
    [SerializeField] private GameObject _fireBall;
    [SerializeField] private Transform _shootPoint;


    //fireball firerate values
    [Header("fireball fire rate values")]
    [SerializeField] private float _timeBetweenFireballs = 2;
    [SerializeField] private float _initalTimeBetweenFireballs;


    /// <summary>
    /// Assigns all needed references to the proper variables.
    /// </summary>
    private void Awake()
    {
        _currentMonsterState = MagmaMonsterState.FlyPath;
        _agent = GetComponent<NavMeshAgent>();
        //sets the current patrol point
        _destination = _patrolPositions[_currentPosition].position;
        _agent.destination = _destination;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();

        //sets the inital time for the time between fireballs 
        _initalTimeBetweenFireballs = _timeBetweenFireballs;
    }

    /// <summary>
    /// Each frame call the Magma Monster Finite State Machine and the timer for the Magma Monster attacks
    /// </summary>
    void FixedUpdate()
    {
        MagmaMonsterFSM();
        _timeBetweenFireballs -= 1 * Time.deltaTime;
    }

    /// <summary>
    /// Switch statements helps control the flow bewtween all Magma Monster states
    /// </summary>
    void MagmaMonsterFSM()
    {
        switch (_currentMonsterState)
        { 
            case MagmaMonsterState.FlyPath:
                HandleFlyPath();
                break;
            case MagmaMonsterState.Chase:
                HandleChase();
                break;
            case MagmaMonsterState.Attack:
                HandleAttack();
                break;
            default: 
                break;
        }
    }

    //method to be called when wanting to change states
    void ChangeState(MagmaMonsterState newState) => _currentMonsterState = newState;



    /// <summary>
    /// all these handlers help call and control the flow bewtween states
    /// </summary>
    //Handlers------------------------------

    /// <summary>
    /// If the magma monster  does not 'sense' the player then continue calling 'flypath' to transistion between points.
    /// Else, the enemy senses the player, thus call the Chase state
    /// </summary>
    void HandleFlyPath()
    {
        if (SenseEnemy() == false) FlyPath();
        else ChangeState(MagmaMonsterState.Chase);
    }


    /// <summary>
    /// If the magma monster  does 'senses' and 'CANT attack' the player then continue calling 'chase' to follow the player
    /// Else if, the enemy senses the player and 'CAN attack', then call the Attack state.
    /// If both 'SenseEnemy' and 'canAttack' both return false, then continue the flightpath
    /// </summary>
    void HandleChase()
    {
        if (SenseEnemy() == true && CanAttack() == false) Chase();
        else if (SenseEnemy() == true && CanAttack() == true) ChangeState(MagmaMonsterState.Attack);
        else ChangeState(MagmaMonsterState.FlyPath);
    }

    /// <summary>
    /// If the enemy senses the player and 'CAN attack', then continue to call the Attack state.
    /// Else call the chase state as the player will still be in view of the Magma Monster enemy.
    /// </summary>
    void HandleAttack()
    {
        if (SenseEnemy() == true && CanAttack() == true) Attack();
        else ChangeState(MagmaMonsterState.Chase);
    }

    //End of handlers-----------------------

    /// <summary>
    /// The functionality implementations for the states are called by their respective handlers.
    /// When called they start the functionality for their respective states.
    /// </summary>




    //Functionality-------------------------

    /// <summary>
    /// Fly path uses the AI pathfing package to move the player between points.
    /// </summary>
    void FlyPath()
    {

        //when the enemy gets pretty close to a patrol point (doesn't need to be exact), then can go the next point.
        if (Vector3.Distance(_agent.transform.position, _patrolPositions[_currentPosition].position) < .2f)
        {
            //sets the current point.
            //this helps loop through all the points regardless of how many we have
            _currentPosition = (_currentPosition + 1) % _patrolPositions.Count;
        }
        //let the navmesh agent go to the current point that is assigned
        _agent.destination = _patrolPositions[_currentPosition].position;
    }

    /// <summary>
    /// Chase uses the AI pathfinding package to follow the player
    /// </summary>
    void Chase()
    {
        //set the agent destination to the player position
        _agent.destination = _player.position;
        //assigns the rotation of the _agent to look at the player
        _agent.transform.LookAt(_player.transform.position);
    }


    /// <summary>
    /// Stops the agent movement when the enemy attacks and lets the enemy still look at the player
    /// Also set the trigger for the attack animation and instantiates a fireball object. 
    /// </summary>
    void Attack()
    {
        //set the agent to it's current position so it stops moving.
        _agent.destination = _agent.transform.position;
        //make the enemy look at the player
        _agent.transform.LookAt(_player.transform.position);
        
        //set animation on for the enemy attack
        _animator.SetTrigger("attack");

        //checks if the time between fireballs is
        //less than or equal to zero to provide adequate time between attacks
        if(_timeBetweenFireballs <= 0)
        {
            // instantiate fireball
            Instantiate(_fireBall, _shootPoint.transform.position, _fireBall.transform.rotation);
            //resets the timer
            _timeBetweenFireballs = _initalTimeBetweenFireballs;
        }
    }

    //End of functionality------------------


    /// <summary>
    /// The functionality checks provide the a way to examine whether the states can transition.
    /// </summary>
    //Functionality checks------------------
    /// <summary>
    /// Checks if the enemy can sense the player by calculating the distance between the magma enemy and player,
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

    /// <summary>
    /// Checks if the enemy can atack the player by calculating the distance between the magma enemy and player,
    /// and assessing if the player is close enough to be attacked
    /// </summary>
    /// <returns> true or false if the enemy can attack the player</returns>
    bool CanAttack()
    {
        //if distance between player and enemy is below the attack radius for the player then enemy can attack the player
        //so return true...
        if (Vector3.Distance(_player.position, _agent.transform.position) < _attackRadius) return true;
        //...otherwise return false
        else return false;
    }

    //End of functionality checks-----------


    /// <summary>
    /// OnDrawGizmos draw the sense and attack radisues in the editor for testing. Fow now, 
    /// it also displays the line between patrol points
    /// </summary>
    //OnDrawGizmos--------------------------
    //visual lines in editor to see the sense radius for testing the enemy
    private void OnDrawGizmos()
    {
        //displays the line between patrol points
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_patrolPositions[0].position, _patrolPositions[1].position);

        Gizmos.color = Color.blue;
        //shows player detection radius radius
        Gizmos.DrawWireSphere(this.transform.position, _awarenessRadius);


        Gizmos.color = Color.yellow;
        //shows attack radius
        Gizmos.DrawWireSphere(this.transform.position, _attackRadius);
    }
    //End of OnDrawGizmos-------------------
}