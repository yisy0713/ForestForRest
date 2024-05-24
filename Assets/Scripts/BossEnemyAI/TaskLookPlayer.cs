using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorTree;

public class TaskLookPlayer : Node
{
    private Transform _transform;

    public TaskLookPlayer(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("Look!!");

        Transform target = (Transform)GetData("target");

        Vector3 direction = (target.position - _transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, Time.deltaTime * 5f);

        state = NodeState.RUNNING;
        return state;
    }
}