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

    public TaskGoToTarget(Transform transform, float speed, NavMeshAgent navMeshAgent)
    {
        _transform = transform;
        _speed = speed;
        _navMeshAgent = navMeshAgent;
        Debug.Log("TaskGoToTarget 생성자 호출, navMesh할당");
    }

    public override NodeState Evaluate()
    {
        _navMeshAgent.speed = _speed;

        //Debug.Log("고투타겟" + _speed);

        if (_navMeshAgent.isStopped)
            _navMeshAgent.isStopped = false;

        Transform target = (Transform)GetData("target");
        Vector3 directionToTarget = (target.position - _transform.position).normalized;

        if (Vector3.Distance(_transform.position, target.position) > 0.01f)
        {
            _navMeshAgent.SetDestination(target.position);
        }

        state = NodeState.RUNNING;
        return state;
    }
}