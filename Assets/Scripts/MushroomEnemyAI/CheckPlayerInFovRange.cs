using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckPlayerInFovRange : Node
{
    private static int _playerLayerMask = 1 << 8;

    private Transform _transform;
    private Animator _animator;
    private float _range;

    public CheckPlayerInFovRange(Transform transform, float range)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
        _range = range;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        bool findPlayer = Physics.CheckSphere(_transform.position, _range, _playerLayerMask);

        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, _range, _playerLayerMask);
            
            if (findPlayer)
            {
                parent.parent.SetData("target", colliders[0].transform);
                state = NodeState.SUCCESS;
                return state;
            }
        }
        else
        {
            if (findPlayer)
            {
                state = NodeState.SUCCESS;
                return state;
            }
            else
            {
                parent.parent.SetData("target", null);
                state = NodeState.FAILURE;
                return state;
            }
        }

        state = NodeState.FAILURE;
        return state;
    }
}