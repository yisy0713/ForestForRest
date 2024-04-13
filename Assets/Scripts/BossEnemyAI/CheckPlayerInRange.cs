using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckPlayerInRange : Node
{
    private static int _playerLayerMask = 1 << 8;

    private Transform _transform;
    private Animator _animator;
    private float _range;

    public CheckPlayerInRange(Transform transform, float range)
    {
        _transform = transform;
        _range = range;
        _animator = transform.GetComponent<Animator>();
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
                Debug.Log("isInRange!");
                parent.parent.SetData("target", colliders[0].transform);
                //_animator.SetBool("Running", true);
                state = NodeState.SUCCESS;
                return state;
            }
        }
        else
        {
            if (findPlayer)
            {
                Debug.Log("isInRange!");
                state = NodeState.SUCCESS;
                return state;
            }
            else
            {
                Debug.Log("isNotInRange!");
                parent.parent.SetData("target", null);
                state = NodeState.FAILURE;
                return state;
            }
        }
        Debug.Log("isNotAttackRange!");
        state = NodeState.FAILURE;
        return state;
    }

}
