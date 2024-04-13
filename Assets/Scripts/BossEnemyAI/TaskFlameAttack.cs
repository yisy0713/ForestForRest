using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskFlameAttack : Node
{
    private Animator _animator;

    public TaskFlameAttack(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Debug.Log("FlameAttack");

        state = NodeState.RUNNING;
        return state;
    }
}
