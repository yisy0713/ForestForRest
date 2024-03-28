using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskPatrol : Node
{
    private Transform _transform;
    private Transform[] _wayPoints;
    private Animator _animator;

    private int _currentWaypointIndex = 0;

    private float _waitTime = 3f;
    private float _waitCounter = 0f;
    private bool _wating = false;

    public TaskPatrol(Transform transform, Transform[] wayPoints)
    {
        _transform = transform;
        _wayPoints = wayPoints;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        if (_wating)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _wating = false;
                _animator.SetBool("Walking", true);
            }
        }
        else
        {
            Transform wp = _wayPoints[_currentWaypointIndex];
            if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
            {
                _transform.position = wp.position;
                _waitCounter = 0f;
                _wating = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _wayPoints.Length;
                _animator.SetBool("Walking", false);
            }
            else
            {
                _transform.position = Vector3.MoveTowards(_transform.position, wp.position, MushroomEnemyAI.speed * Time.deltaTime);
                _transform.LookAt(wp.position);
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}

