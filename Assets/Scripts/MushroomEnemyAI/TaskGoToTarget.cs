using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskGoToTarget : Node
{
    private Transform _transform;
    private Animator _animator;
    private Rigidbody _rigid;
    private float _speed;

    public TaskGoToTarget(Transform transform, Rigidbody rigid, float speed)
    {
        _transform = transform;
        //_animator = transform.GetComponent<Animator>();
        _rigid = rigid;
        _speed = speed;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        Vector3 directionToTarget = (target.position - _transform.position).normalized;

        //_animator.SetBool("Walking", false);
        //_animator.SetBool("Waiting", false);
        //_animator.SetBool("Running", true);

        if (Vector3.Distance(_transform.position, target.position) > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            Quaternion _smoothRotation = Quaternion.Lerp(_transform.rotation, targetRotation, 0.07f);
            _rigid.MoveRotation(_smoothRotation);
            _rigid.MovePosition(_transform.position + (_transform.forward * _speed * Time.deltaTime));
        }

        state = NodeState.RUNNING;
        return state;
    }
}