using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskTailAttack : Node
{
    private StatusUI _playerStatus;
    private float _damage;
    
    public TaskTailAttack(Transform transform, float damage)
    {
        _playerStatus = Object.FindObjectOfType<StatusUI>();
        _damage = damage;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("TailAttack");

        _playerStatus.DecreaseHp(_damage);

        state = NodeState.RUNNING;
        return state;
    }
}
