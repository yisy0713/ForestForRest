using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskTailAttack : Node
{
    private Animator _animator;
    
    public TaskTailAttack(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Debug.Log("TailAttack");

        state = NodeState.RUNNING;
        return state;
    }
}
