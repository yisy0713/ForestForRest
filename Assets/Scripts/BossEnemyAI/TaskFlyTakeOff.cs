using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskFlyTakeOff : Node
{
    private Transform _transform;
    private BoxCollider _boxCollider;
    public TaskFlyTakeOff(Transform transform)
    {
        _transform = transform;

        _boxCollider = transform.GetComponent<BoxCollider>();
    }

    public override NodeState Evaluate()
    {
        _boxCollider.center = new Vector3(0, 5.7f, 0);

        state = NodeState.SUCCESS;
        return state;
    }
}
