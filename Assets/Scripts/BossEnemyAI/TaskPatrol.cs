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
    private bool _waiting = false;

    private float _speed;

    private Vector3 direction;

    private Vector3 target;

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
        _navMeshAgent.speed = _speed;
        //_animator.SetBool("Fly", false);
        //_animator.SetBool("Sleeping", false);

        if (_waiting)        // 기다리기
        {
            _navMeshAgent.isStopped = true;
            _animator.SetBool("Idle", true);

            foreach (AnimatorControllerParameter parameter in _animator.parameters)
            {
                if (parameter.name == "Idle")
                    continue;

                _animator.SetBool(parameter.name, false);
            }

            _waitCounter += Time.deltaTime;

            if (_waitCounter >= _waitTime)
            {
                _waiting = false;
                _walkCounter = 0f;
            }
        }
        else
        {
            _walkCounter += Time.deltaTime;
            if (_walkCounter >= _walkTime)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 10f; // 10 단위 거리만큼 이동
                randomDirection += _transform.position;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, 1.0f, NavMesh.AllAreas))
                {
                    target = hit.position; // 유효한 위치로 설정
                    _navMeshAgent.SetDestination(target);
                }

                _walkCounter = 0f;
                _waitCounter = 0f;
                _waiting = true;

                state = NodeState.RUNNING;
                return state;
            }
            else     // 걷기 
            {
                _navMeshAgent.isStopped = false;
                _animator.SetBool("Walk", true);

                foreach (AnimatorControllerParameter parameter in _animator.parameters)
                {
                    if (parameter.name == "Walk")
                        continue;

                    _animator.SetBool(parameter.name, false);
                }

                if (_navMeshAgent.remainingDistance < 0.5f)
                {
                    Vector3 randomDirection = Random.insideUnitSphere * 10f; // 10 단위 거리만큼 이동
                    randomDirection += _transform.position;

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomDirection, out hit, 1.0f, NavMesh.AllAreas))
                    {
                        target = hit.position; // 유효한 위치로 설정
                        _navMeshAgent.SetDestination(target);
                    }
                }
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
