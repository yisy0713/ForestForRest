using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckAttackTimer : Node
{
    private float _attackTimer;
    private float _currTimer = 0f;

    public CheckAttackTimer(float attackTimer)
    {
        _attackTimer = attackTimer;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(_currTimer);
        if (_currTimer < _attackTimer)
        {
            _currTimer += Time.deltaTime;
            state = NodeState.FAILURE;
            return state;
        }
        _currTimer = 0;
        state = NodeState.SUCCESS;
        return state;
    }
}
