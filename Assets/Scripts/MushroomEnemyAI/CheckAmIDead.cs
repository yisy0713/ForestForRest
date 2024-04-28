using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckAmIDead : Node
{
    private Transform _transform;
    private EnemyManager _enemyManager;
    private Animator _animator;

    public CheckAmIDead(Transform transfrom)
    {
        _transform = transfrom;
        _enemyManager = _transform.GetComponent<EnemyManager>();
        _animator = _transform.GetComponent<Animator>();
    }
    public override NodeState Evaluate()
    {
        if (_enemyManager.enemyDead)
        {
            //_animator.SetBool("isDead", true);
               
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }

}
