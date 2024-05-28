using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class CheckPlayerInAttactRange : Node
{
    private Transform _transform;
    private Animator _animator;

    public CheckPlayerInAttactRange(Transform transfrom)
    {
        _transform = transfrom;
        _animator = transfrom.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;
        if(Vector3.Distance(_transform.position, target.position) <=1.2f)
        {
            //_animator.SetBool("Attacking", true);
            _animator.SetBool("Walking", false);
            _animator.SetBool("Running", false);

            state = NodeState.SUCCESS;
            return state;
        }

        //_animator.SetBool("Attacking", false);

        state = NodeState.FAILURE;
        return state;
    }
}
