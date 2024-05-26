using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckOnGround : Node
{
    private Transform _transform;
    private Animator _animator;

    public CheckOnGround(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {

        if(_animator.GetBool("Idle") || _animator.GetBool("Walk") || _animator.GetBool("Run"))
        {
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.FAILURE;
        return state;
    }

}