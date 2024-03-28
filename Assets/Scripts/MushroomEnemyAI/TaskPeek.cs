using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskPeek : Node
{
    private Animator _animator;

    private float _peekTime = 2f;
    private float _peekCounter = 0f;

    public TaskPeek(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
    }
    public override NodeState Evaluate()
    {
        _peekCounter += Time.deltaTime;
        if (_peekCounter >= _peekTime)
        {
            _peekCounter = 0f;
            _animator.SetBool("Peeking", false);

            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            _animator.SetBool("Peeking", true);
        }

        state = NodeState.RUNNING;
        return state;
    }
}
