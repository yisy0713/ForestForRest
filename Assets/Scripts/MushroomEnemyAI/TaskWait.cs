using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskWait : Node
{
    private Animator _animator;

    private float _waitTime = 200f;
    private float _waitCounter = 0f;

    public TaskWait(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
    }
    public override NodeState Evaluate()
    {
        _waitCounter += Time.deltaTime;
        if (_waitCounter >= _waitTime)
        {
            _waitCounter = 0f;
            _animator.SetBool("Waiting", false);

            state = NodeState.FAILURE;
            return state;
        }
        else
        {
            _animator.SetBool("Waiting", true);
        }

        state = NodeState.RUNNING;
        return state;
    }
}