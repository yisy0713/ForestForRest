using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorTree;

public class TaskGoToTarget : Node
{
    private Transform _transform;
    private float _speed;
    private NavMeshAgent _navMeshAgent;
    private float _stopDistance;

    public TaskGoToTarget(Transform transform, float speed, NavMeshAgent navMeshAgent, float stopDistance)
    {
        _transform = transform;
        _speed = speed;
        _navMeshAgent = navMeshAgent;
        _stopDistance = stopDistance;
        Debug.Log("TaskGoToTarget 생성자 호출, navMesh할당");
    }

    public override NodeState Evaluate()
    {
        _navMeshAgent.speed = _speed;

        if(_navMeshAgent.isStopped)
            _navMeshAgent.isStopped = false;

        Transform target = (Transform)GetData("target");
        Vector3 directionToTarget = (target.position - _transform.position).normalized;

        if (Vector3.Distance(_transform.position, target.position) > 0.01f)
        {
            _navMeshAgent.SetDestination(target.position);

            if (Vector3.Distance(_transform.position, target.position) < _stopDistance - 0.2f)
                _navMeshAgent.isStopped = true;
        }

        state = NodeState.RUNNING;
        return state;
    }
}