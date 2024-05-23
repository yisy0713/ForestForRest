using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskWaitAttack : Node
{
    private Animator _animator;
    public TaskWaitAttack(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        //Debug.Log("AttackCoolTime");

        state = NodeState.RUNNING;
        return state;
    }
}
