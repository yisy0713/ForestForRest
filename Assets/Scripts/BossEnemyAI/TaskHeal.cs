using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorTree;

public class TaskHeal : Node
{
    private Transform _transform;
    private EnemyManager _enemyManager;
    private int _hp;

    // Start is called before the first frame update
    public TaskHeal(Transform tranform)
    {
        _transform = tranform;
        _enemyManager = _transform.GetComponent<EnemyManager>();
    }

    public override NodeState Evaluate()
    {
        _enemyManager.EnemyIncreaseHp(Time.deltaTime);

        state = NodeState.RUNNING;
        return state;
    }
}
