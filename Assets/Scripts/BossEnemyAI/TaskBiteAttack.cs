using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskBiteAttack : Node
{
    // Start is called before the first frame update
    private Animator _animator;

    public TaskBiteAttack(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Debug.Log("BiteAttack");

        state = NodeState.RUNNING;
        return state;
    }
}
