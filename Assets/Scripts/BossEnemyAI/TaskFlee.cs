using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorTree;

public class TaskFlee : Node
{
    private Transform _transform;
    private float _speed;
    private NavMeshAgent _navMeshAgent;

    public TaskFlee(Transform transform, float speed, NavMeshAgent navMeshAgent)
    {
        _transform = transform;
        _speed = speed;
        _navMeshAgent = navMeshAgent;
        //Debug.Log("TaskFlee 생성자 호출, navMesh 할당");
    }

    public override NodeState Evaluate()
    {
        _navMeshAgent.speed = _speed;

        if (_navMeshAgent.isStopped)
            _navMeshAgent.isStopped = false;

        Transform target = (Transform)GetData("target");
        Vector3 directionToFlee = (_transform.position - target.position).normalized;

        Vector3 fleePosition = _transform.position + directionToFlee * 10f;
        _navMeshAgent.SetDestination(fleePosition);

        state = NodeState.RUNNING;
        return state;
    }
}
