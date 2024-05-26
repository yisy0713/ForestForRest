using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskFlyLand : Node
{
    private Transform _transform;
    private BoxCollider _boxCollider;
    public TaskFlyLand(Transform transform)
    {
        _transform = transform;

        _boxCollider = transform.GetComponent<BoxCollider>();
    }

    public override NodeState Evaluate()
    {
        _boxCollider.center = new Vector3(0, 1.7f, 0);

        state = NodeState.FAILURE;
        return state;
    }
}
