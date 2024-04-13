using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

public class SkeletonFiniteStateMachine : MonoBehaviour
{
    public enum SkeletonState { Patrol, Chase, Explode }

    [Header("Current Skeleton State")]
    [SerializeField] private SkeletonState _currentSkeletonState;

    [Header("Skeleton Awareness Variables")]
    [SerializeField] private float _awarenessRadius = 20.0f;

    [Header("Skeleton Speed Variables")]
    [SerializeField] private float _patrolSpeed = 5.0f;
    [SerializeField] private float _chaseSpeed = 10.0f;

    [Header("Skeleton Patrol Points")]
    [SerializeField] private int _currentPosition = 0;
    [SerializeField] private List<Transform> _patrolPositions;

    [Header("Nav Mesh Variables")]
    private NavMeshAgent _agent;

    [Header("Animator for Skeleton")]
    private Animator _animator;

    [Header("Explosion Capability Check")]
    [SerializeField] private bool _canExplode = false;

    [Header("Bomb and Explosion References")]
    [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _bombExplosion;

    [Header("Audio Sources and Clips")]
    [SerializeField] private AudioSource walkingAudioSource;
    [SerializeField] private AudioSource explosionAudioSource;
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip explosionSound;

    private bool _isBombExploded = false;
    private Transform _player;
    private bool _isAlive = true;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (_player == null)
        {
            Debug.LogError("Player tag is not found in the scene. Make sure your player is tagged correctly.");
        }

        SetupAudioSources();
    }

    void SetupAudioSources()
    {
        if (walkingAudioSource != null)
        {
            walkingAudioSource.spatialBlend = 1.0f; // Set the AudioSource to be fully spatial.
            walkingAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            walkingAudioSource.maxDistance = 15f; // Set this to the desired max distance for hearing footsteps.
        }

        if (explosionAudioSource != null)
        {
            explosionAudioSource.spatialBlend = 1.0f;
            explosionAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            explosionAudioSource.maxDistance = 20f; // Explosions can be heard from farther away.
        }
    }

    void FixedUpdate()
    {
        if (_isAlive)
        {
            SkeletonFSM();
        }
    }

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
        }
    }

    void HandlePatrol()
    {
        if (!SenseEnemy())
        {
            _animator.SetBool("isWalking", true);
            _animator.SetBool("isRunning", false);
            if (walkingAudioSource != null && !walkingAudioSource.isPlaying)
                walkingAudioSource.PlayOneShot(walkingSound, 0.7f);
            Patrol();
        }
        else
        {
            if (walkingAudioSource != null)
                walkingAudioSource.Stop();
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isWalking", false);
            ChangeState(SkeletonState.Chase);
        }
    }

    void HandleChase()
    {
        if (_canExplode)
        {
            _animator.SetBool("isExploding", true);
            ChangeState(SkeletonState.Explode);
        }
        else if (SenseEnemy())
        {
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isWalking", false);
            Chase();
        }
        else
        {
            _animator.SetBool("isWalking", true);
            _animator.SetBool("isRunning", false);
            ChangeState(SkeletonState.Patrol);
        }
    }

    void HandleExplode()
    {
        Explode();
    }

    void ChangeState(SkeletonState newState)
    {
        _currentSkeletonState = newState;
    }

    void Patrol()
    {
        if (Vector3.Distance(_agent.transform.position, _patrolPositions[_currentPosition].position) < .2f)
            _currentPosition = (_currentPosition + 1) % _patrolPositions.Count;
        _agent.speed = _patrolSpeed;
        _agent.destination = _patrolPositions[_currentPosition].position;
    }

    void Chase()
    {
        _agent.speed = _chaseSpeed;
        _agent.destination = _player.position;
    }

    void Explode()
    {
        _agent.isStopped = true;
        if (!_isBombExploded)
        {
            _isBombExploded = true;
            Instantiate(_bombExplosion, this.transform.position, this.transform.rotation);
            if (explosionAudioSource != null)
                explosionAudioSource.PlayOneShot(explosionSound);
        }
        Destroy(_bomb);
        _isAlive = false;
        Destroy(this.gameObject, 1); // Ensures this object is considered destroyed after 1 second.
    }

    bool SenseEnemy()
    {
        return _player != null && Vector3.Distance(_player.position, _agent.transform.position) < _awarenessRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            if (!_canExplode)
            {
                other.gameObject.GetComponent<PlayerHealth>().LoseHealth();
            }
            _canExplode = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_patrolPositions.Count >= 4)
        {
            Gizmos.DrawLine(_patrolPositions[0].position, _patrolPositions[1].position);
            Gizmos.DrawLine(_patrolPositions[1].position, _patrolPositions[2].position);
            Gizmos.DrawLine(_patrolPositions[2].position, _patrolPositions[3].position);
            Gizmos.DrawLine(_patrolPositions[3].position, _patrolPositions[0].position);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _awarenessRadius);
    }
}
