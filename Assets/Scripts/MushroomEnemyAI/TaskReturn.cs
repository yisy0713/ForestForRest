using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskReturn : Node
{
    private Transform _transform;
    private Transform _returnPoints;
    private Animator _animator;

    public TaskReturn(Transform transform, Transform originPoints)
    {
        _transform = transform;
        _returnPoints = originPoints;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        Transform wp = _returnPoints;

        Debug.Log("돌아가자");
            
        if (Vector3.Distance(_transform.position, wp.position) < 0.1f)
        {
            _transform.position = wp.position;
            _animator.SetBool("Walking", false);
            _animator.SetBool("Waiting", true);
            state = NodeState.RUNNING;
        }
        else
        {
            _animator.SetBool("Walking", true);
            _transform.position = Vector3.MoveTowards(_transform.position, wp.position, MushroomEnemyAI.speed * Time.deltaTime);
            _transform.LookAt(wp.position);
        }

        state = NodeState.RUNNING;
        return state;
    }
}
