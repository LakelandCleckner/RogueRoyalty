/*
 * Source File Name: TrollFiniteStateMachine.cs
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

using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// The purpose of this class is the define the behaviour of the Troll and all the flow between all its behavioral states.
/// </summary>
public class TrollFiniteStateMachine : MonoBehaviour
{
    //All Troll states
    public enum TrollState { Guard, Approach, Stunned, Attack }

    //current troll state
    [Header("Current Troll state")]
    [SerializeField] private TrollState _currentTrollState;

    //awarness variables (in the nav mesh agant component)
    [Header("Troll awareness and attack radius variables")]
    [SerializeField] private float _awarenessRadius = 5.0f;
    [SerializeField] private float _attackRadius = 2.0f;

    //speed variables (in the nav mesh agant component)
    [Header("Troll approaching speed variables")]
    [SerializeField] private float _approachSpeed = 2.0f;
    [SerializeField] private float _returnSpeed = 1.0f;

    //reference to the player
    [Header("Player Transform reference")]
    private Transform _player;

    //Nav mesh related variables
    [Header("Nav Mesh related variables for AI pathfinding")]
    private NavMeshAgent _agent;

    [Header("Guard point variables")]
    [SerializeField] private Transform _guardPoint;
    [SerializeField] private Transform _guardLookPoint;

    //Animator for the Troll
    [Header("Animator for Troll")]
    private Animator _animator;

    //check for if the troll is stunned
    [Header("Check if Troll is stunned")]
    [SerializeField] private bool _isStunned = false;

    //reference to the axe collider
    [Header("Reference to the troll axe collider")]
    [SerializeField] private GameObject _axeCollider;


    /// <summary>
    /// Assigns all needed references to the proper variables.
    /// </summary>
    void Awake()
    {
        _currentTrollState = TrollState.Guard;
        _agent = GetComponent<NavMeshAgent>();
        
        //find the player
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        //get the animator for the troll
        _animator = GetComponent<Animator>();
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
    /// <param name="newState">This parameter is the new state wanting to be called</param
    void ChangeState(TrollState newState) => _currentTrollState = newState;


    /// <summary>
    /// all these handlers help call and control the flow bewtween states
    /// </summary>
    //Handlers------------------------------


    /// <summary>
    /// handles the flow of the Guard state and the transition to other states
    /// </summary>
    void HandleGuard()
    {
        //if stunned change state to stunned state --> done first as any state can call stunned
        if (_isStunned)
        {
            ChangeState(TrollState.Stunned);
        }
        //else if cant sense enemy change state to guard state
        else if (SenseEnemy() == false)
        {
            //...if no player guard
            Guard();
        }
        //else if can sense enemy change state to approach state
        else if (SenseEnemy() == true)
        {
            //...then approach the player
            ChangeState(TrollState.Approach);
        }
    }

    /// <summary>
    /// handles the flow of the Approach state and the transition to other states
    /// </summary>
    void HandleApproach()
    {
        //if stunned change state to stunned state --> done first as any state can call stunned
        if (_isStunned)
        {
            ChangeState(TrollState.Stunned);
        }
        //else if can sense enemy but cant attack change state to approach state
        else if (SenseEnemy() == true && CanAttack() == false)
        {
            Approach();
        }
        //else if cant sense enemy and cant attack, change state to guard state
        else if (SenseEnemy() == false && CanAttack() == false)
        {
            ChangeState(TrollState.Guard);
        }
        //else if can sense enemy and can attack change state to attack state
        else if (CanAttack() == true && SenseEnemy() == true)
        {
            ChangeState(TrollState.Attack);
        }
    }


    /// <summary>
    /// handles the flow of the Stunned state and the transition to other states
    /// </summary>
    void HandleStunned()
    {
        //if stunned change state to stunned state --> done first as any state can call stunned
        if (_isStunned == true)
        {
            //if the Troll is stunned set _isStunned to false so Stunned is only called once
            _isStunned = false;
            Stunned();
        }
        //else if can sense enemy but cant attack, change state to approach state
        else if (SenseEnemy() == true && CanAttack() == false) 
        {
            ChangeState(TrollState.Approach);
        }
        //else if cant sense and cant attack enemy change state to guard state
        else if (SenseEnemy() == false && CanAttack() == false)
        {
            ChangeState(TrollState.Guard);
        }
        //else if can sense enemy and can attack, change state to attack state
        else if (CanAttack() == true && SenseEnemy() == true)
        {
            ChangeState(TrollState.Attack);
        }
    }


    /// <summary>
    /// handles the flow of the Attack state and the transition to other states
    /// </summary>
    void HandleAttack()
    {
        //if stunned change state to stunned state --> done first as any state can call stunned
        if (_isStunned == true)
        {
            ChangeState(TrollState.Stunned);
        }
        //else if can sense enemy and can attack, change state to attack state
        else if (CanAttack() == true && SenseEnemy() == true)
        {
            Attack();
        }
        //else if can sense enemy but cant attack, change state to approach state
        else if (CanAttack() == false && SenseEnemy() == true)
        {
            ChangeState(TrollState.Approach);
        }
    }


    //End of handlers-----------------------

    /// <summary>
    /// The functionality implementations for the states are called by their respective handlers.
    /// When called they start the functionality for their respective states.
    /// </summary>

    //Functionality-------------------------

    /// <summary>
    /// This is the Guard state implementation for the Troll.
    /// </summary>
    void Guard()
    {
        //set the return speed
        _agent.speed = _returnSpeed;
        //set the agent to go back to the guard point with AI pathfinding
        _agent.destination = _guardPoint.position;
        //make sure the attck animation bool is false
        _animator.SetBool("canAttack", false);
        //make sure the the axe collider is off
        _axeCollider.SetActive(false);

        //check if the Troll is at the guard point...
        if (Vector3.Distance(_agent.transform.position, _guardPoint.transform.position) < 0.2f)
        {
            //if so set the needToMove animation bool off
            _animator.SetBool("needToMove", false);
            //make sure the agent is looking at the guardLookPoint when he is stationed properly
            _agent.transform.LookAt(_guardLookPoint.transform.position);
        }
        ///...otherwise the troll is not back at his guard point so...
        else
        {
            //...make sure the needToMove animation bool is still true to display the running animation.
            _animator.SetBool("needToMove", true);
        }
    }

    /// <summary>
    /// This is the Approach state implementation for the Troll.
    /// </summary>
    void Approach()
    {
        //set the speed to approach the player
        _agent.speed = _approachSpeed;
        //set the agent destination as the player for AI pathfinding
        _agent.destination = _player.transform.position;
        //make sure the axecollider that can damage the player is off still
        _axeCollider.SetActive(false);

        //set the needToMove bool as true and the canAttack
        //bool is false to keep playing the run animation for the troll
        _animator.SetBool("needToMove", true);
        _animator.SetBool("canAttack", false);
    }

    /// <summary>
    /// This is the Stunned state implementation for the Troll.
    /// </summary>
    void Stunned()
    {
        //briefly set the destination to the current position to 'stop' the troll briefly
        _agent.destination = _agent.transform.position;
        //make sure the axecollider that can damage the player is off still
        _axeCollider.SetActive(false);

        //set the isStunned trigger to play the troll 'getting hit' animation
        _animator.SetTrigger("isStunned");
    }

    /// <summary>
    /// This is the Attack state implementation for the Troll.
    /// </summary>
    void Attack()
    {
        //set the destination to the current position to 'stop' the troll
        _agent.destination = _agent.transform.position;
        //make the AI pathfinding agent look at the player
        _agent.transform.LookAt(_player.transform.position);

        //set the axecollider to hit the player to active or on
        _axeCollider.SetActive(true);

        //set the canAttack animation bool to true and the needToMove
        //animation to false to display the troll attack animation
        _animator.SetBool("canAttack", true);
        _animator.SetBool("needToMove", false);
    }


    //End of functionality------------------

    /// <summary>
    /// The functionality checks provide the a way to examine whether the states can transition.
    /// </summary> 

    //Functionality checks------------------

    /// <summary>
    /// Checks if the troll can sense the player by calculating the distance between the itself and player,
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
    /// Checks if the troll can atack the player by calculating the distance between the itself and player,
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


    //checks if the troll was hit by the player projectile, to tell the troll FSM that the troll is stunned
    private void OnTriggerEnter(Collider other)
    {
        //if the troll was hit by the Projectile tag... 
        if (other.gameObject.CompareTag("Projectile"))
        {
            //...then set isStunned to true to make sure the Troll FSM updates accordingly
            _isStunned = true;
        }
    }

    //End of functionality checks-----------

    /// <summary>
    /// OnDrawGizmos draws the awareness radius and attack radius for the Troll in the editor for testing. 
    /// </summary>
    //OnDrawGizmos--------------------------
    //visual lines in editor to see the sense radius for testing the enemy
    private void OnDrawGizmos()
    {
        //set gizmo color
        Gizmos.color = Color.yellow;
        //shows troll attack radius radius
        Gizmos.DrawWireSphere(this.transform.position, _attackRadius);

        //set gizmo color
        Gizmos.color = Color.blue;
        //shows troll detection radius radius
        Gizmos.DrawWireSphere(this.transform.position, _awarenessRadius);
    }
    //End of OnDrawGizmos-------------------
}
