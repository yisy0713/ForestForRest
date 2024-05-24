using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskFireBallAttack : Node
{
    private Transform _transform;
    private GameObject _poisionBall;

    public TaskFireBallAttack(Transform transform, GameObject poisonBall)
    {
        _transform = transform;
        _poisionBall = poisonBall;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("FireBallAttack");

        Vector3 FirePosition = new Vector3(_transform.position.x, _transform.position.y + 3f, _transform.position.z);
        GameObject instantPoisionBall = Object.Instantiate(_poisionBall, FirePosition, _transform.rotation);

        state = NodeState.RUNNING;
        return state;
    }
}
