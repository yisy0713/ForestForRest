using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskAttack : Node
{
    private StatusUI _playerStatus;

    private float _damage;

    public TaskAttack(Transform transform, float damage)
    {
        _playerStatus = Object.FindObjectOfType<StatusUI>();
        _damage = damage;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");


        _playerStatus.DecreaseHp(_damage);

        bool playerIsDead = _playerStatus.GetIsDead();
        if (playerIsDead)
        {
            ClearData("target");
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }
}