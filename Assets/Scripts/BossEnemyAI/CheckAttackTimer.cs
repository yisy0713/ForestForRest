using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckAttackTimer : Node
{
    private float _attackTimer;
    private float _timer = 0f;

    public CheckAttackTimer(float attackTimer)
    {
        _attackTimer = attackTimer;
    }

    public override NodeState Evaluate()
    {
        if (_timer < _attackTimer)
        {
            _timer += Time.deltaTime;
            state = NodeState.RUNNING;
            return state;
        }
        _timer = 0;
        state = NodeState.SUCCESS;
        return state;
    }
}
