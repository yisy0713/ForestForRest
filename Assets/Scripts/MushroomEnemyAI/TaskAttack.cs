using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskAttack : Node
{
    private Animator _animator;

    private StatusUI _playerStatus;

    private float _attackTime = 1f;
    private float _attackCounter = 1f;

    private float MushroomAttackPower = 15;

    public TaskAttack(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
        _playerStatus = Object.FindObjectOfType<StatusUI>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        _attackCounter += Time.deltaTime;
        if(_attackCounter >= _attackTime)
        {
            //_animator.SetBool("Running", false);
            //_animator.SetTrigger("Attack");
            _playerStatus.DecreaseHp(MushroomAttackPower);

            bool playerIsDead = _playerStatus.GetIsDead();
            if (playerIsDead)
            {
                ClearData("target");
                //_animator.SetBool("Attacking", false);

                state = NodeState.FAILURE;
                return state;
            }
            else
            {
                _attackCounter = 0f;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
