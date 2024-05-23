using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorTree;

public class SetAnim : Node
{
    private Transform _transform;
    private Animator _animator;
    private string _animParameter;
    //private Rigidbody _rigid;

    public SetAnim(Transform transform, string animParameter)
    {
        _transform = transform;
        //_rigid = rigid;
        _animator = transform.GetComponent<Animator>();
        _animParameter = animParameter;
    }

    public override NodeState Evaluate()
    {

        //Debug.Log( "SetAnim : " + _animParameter);

        _animator.SetBool(_animParameter, true);
        _animator.SetTrigger(_animParameter);

        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            if (parameter.name == _animParameter)
                continue;

            _animator.SetBool(parameter.name, false);
        }

        //Debug.Log("GoToTarget  " + _navMeshAgent.speed);

        //_animator.SetBool("Walking", false);
        //_animator.SetBool("Waiting", false);
        //_animator.SetBool("Running", true);

        state = NodeState.SUCCESS;
        return state;
    }
}