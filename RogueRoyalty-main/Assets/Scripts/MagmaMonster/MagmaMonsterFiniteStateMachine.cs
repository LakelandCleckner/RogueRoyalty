using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
*
*      -> February 24th, 2024: 
*          -Corrected some comments
*          -Changed some SerializeFields to not serializable anymore
*      -> March 28th, 2024:
*          -Refactored the shoot state to use the Object Pooling Pattern using the FireBallPoolManager
*/
/// <summary>
/// Defines the behavior of the Magma Monster and manages its state transitions.
/// </summary>
public class MagmaMonsterFiniteStateMachine : MonoBehaviour
{
    public enum MagmaMonsterState { FlyPath, Chase, Attack }

    [Header("Current Monster State")]
    [SerializeField] private MagmaMonsterState _currentMonsterState;

    [Header("Magma Monster Awareness Variables")]
    [SerializeField] private float _awarenessRadius = 20.0f;
    [SerializeField] private float _attackRadius = 10.0f;

    [Header("Magma Monster Patrol Points")]
    [SerializeField] private int _currentPosition = 0;
    [SerializeField] private List<Transform> _patrolPositions;

    private Transform _player;
    private NavMeshAgent _agent;
    private Animator _animator;

    [Header("Fireball Related References")]
    [SerializeField] private GameObject _fireBall;
    [SerializeField] private Transform _shootPoint;

    [Header("Fireball Fire Rate Values")]
    [SerializeField] private float _timeBetweenFireballs = 2;
    private float _initialTimeBetweenFireballs;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource movingAudioSource;
    [SerializeField] private AudioSource shootingAudioSource;

    private Vector3 _destination;  // Destination used by the NavMeshAgent

    private void Awake()
    {
        _currentMonsterState = MagmaMonsterState.FlyPath;
        _agent = GetComponent<NavMeshAgent>();
        _destination = _patrolPositions[_currentPosition].position; // Initialize destination
        _agent.destination = _destination;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        _initialTimeBetweenFireballs = _timeBetweenFireballs;
    }

    private void FixedUpdate()
    {
        MagmaMonsterFSM();
        _timeBetweenFireballs -= Time.deltaTime;
    }

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
        }
    }

    void HandleFlyPath()
    {
        if (!SenseEnemy())
        {
            FlyPath();
        }
        else
        {
            ChangeState(MagmaMonsterState.Chase);
        }
    }

    void HandleChase()
    {
        if (SenseEnemy() && !CanAttack())
        {
            Chase();
        }
        else if (SenseEnemy() && CanAttack())
        {
            ChangeState(MagmaMonsterState.Attack);
        }
        else
        {
            ChangeState(MagmaMonsterState.FlyPath);
        }
    }

    void HandleAttack()
    {
        if (SenseEnemy() && CanAttack())
        {
            Attack();
        }
        else
        {
            ChangeState(MagmaMonsterState.Chase);
        }
    }

    void FlyPath()
    {
        if (Vector3.Distance(_agent.transform.position, _patrolPositions[_currentPosition].position) < .2f)
        {
            _currentPosition = (_currentPosition + 1) % _patrolPositions.Count;
            _destination = _patrolPositions[_currentPosition].position;
            _agent.destination = _destination;
        }
        if (!movingAudioSource.isPlaying && Vector3.Distance(_player.position, transform.position) < _awarenessRadius)
        {
            movingAudioSource.Play();
        }
    }

    void Chase()
    {
        _agent.destination = _player.position;
        if (!movingAudioSource.isPlaying)
        {
            movingAudioSource.Play();
        }
    }

    void Attack()
    {
        _agent.destination = _agent.transform.position;
        _agent.transform.LookAt(_player.transform.position);
        _animator.SetTrigger("attack");

        if (_timeBetweenFireballs <= 0)
        {
            var fireball = FireBallPoolManager.Instance.GetPrefabFromPool();
            fireball.transform.SetPositionAndRotation(_shootPoint.transform.position, _fireBall.transform.rotation);
            fireball.gameObject.SetActive(true);
            _timeBetweenFireballs = _initialTimeBetweenFireballs;

            if (Vector3.Distance(_player.position, transform.position) < _attackRadius)
            {
                shootingAudioSource.Play();
            }
        }
    }

    bool SenseEnemy()
    {
        return Vector3.Distance(_player.position, _agent.transform.position) < _awarenessRadius;
    }

    bool CanAttack()
    {
        return Vector3.Distance(_player.position, _agent.transform.position) < _attackRadius;
    }

    void ChangeState(MagmaMonsterState newState)
    {
        _currentMonsterState = newState;
    }
}
