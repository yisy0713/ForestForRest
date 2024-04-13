using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskFireBallAttack : Node
{
    private Animator _animator;

    public TaskFireBallAttack(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Debug.Log("FireBallAttack");

        state = NodeState.RUNNING;
        return state;
    }
}
