using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class TaskPatrol : Node
{
    private Transform _transform;
    private Animator _animator;
    private Rigidbody _rigid;
    private NavMeshAgent _navMeshAgent;

    private float _walkTime = 8f;
    private float _walkCounter = 0f;

    private float _waitTime = 5f;
    private float _waitCounter = 0f;
    private bool _wating = false;

    private float _speed;

    private Vector3 direction;

    public TaskPatrol(Transform transform, Rigidbody rigid, float speed, NavMeshAgent navMeshAgent)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
        _rigid = rigid;
        _speed = speed;
        _navMeshAgent = navMeshAgent;
    }

    public override NodeState Evaluate()
    {
        //_animator.SetBool("Fly", false);
        //_animator.SetBool("Sleeping", false);

        if (_wating)        // 기다리기
        {
            _waitCounter += Time.deltaTime;

            if (_waitCounter >= _waitTime)
            {
                _wating = false;
            }
        }
        else
        {
            _walkCounter += Time.deltaTime;
            if (_walkCounter >= _walkTime)
            {
                direction.Set(0f, Random.Range(0f, 360f), 0f);

                _walkCounter = 0f;
                //_animator.SetBool("Walking", false);
                //_animator.SetBool("Waiting", true);

                _waitCounter = 0f;
                _wating = true;

                state = NodeState.RUNNING;
                return state;
            }
            else     // 걷기 
            {
               // _animator.SetBool("Walking", true);
               // _animator.SetBool("Waiting", false);
                Quaternion targetRotation = Quaternion.Euler(0f, direction.y, 0f);
                Quaternion smoothRotation = Quaternion.Lerp(_transform.rotation, targetRotation, 0.01f);
                _rigid.MoveRotation(smoothRotation);
                Vector3 moveDirection = _transform.forward * _speed * Time.deltaTime;
                _navMeshAgent.SetDestination(moveDirection);
                //_rigid.MovePosition(_transform.position + (_transform.forward * _speed * Time.deltaTime));
            }
        }

        Debug.Log("Patrollllllllllllllllllllllllllll");

        state = NodeState.RUNNING;
        return state;
    }
}
