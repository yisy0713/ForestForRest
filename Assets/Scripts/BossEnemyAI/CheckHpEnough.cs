using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckHpEnough : Node
{
    private Transform _transform;
    private EnemyManager _enemyManager;
    private Animator _animator;
    private int _hp;

    public CheckHpEnough(Transform tranform, int hp)
    {
        _transform = tranform;
        _hp = hp;
        _enemyManager = _transform.GetComponent<EnemyManager>();
        _animator = _transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        if (_enemyManager.curHp > _hp)
        {
            Debug.Log("chekHpEnough!!");
            state = NodeState.SUCCESS;
            return state;
        }

        Debug.Log("HpNOTEnough!!");
        //_animator.SetBool("Die", true);
        state = NodeState.FAILURE;
        return state;
    }
}
