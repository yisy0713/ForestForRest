using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskBiteAttack : Node
{
    private StatusUI _playerStatus;
    private float _damage;

    public TaskBiteAttack(Transform transform, float damage)
    {
        _playerStatus = Object.FindObjectOfType<StatusUI>();
        _damage = damage;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("BiteAttack");

        _playerStatus.DecreaseHp(_damage);

        state = NodeState.RUNNING;
        return state;
    }
}
