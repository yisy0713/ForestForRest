using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorTree;

public class TaskGoToTarget : Node
{
    private Transform _transform;
    private Animator _animator;
    private Rigidbody _rigid;
    private float _speed;
    private NavMeshAgent _navMeshAgent;

    public TaskGoToTarget(Transform transform, Rigidbody rigid, float speed, NavMeshAgent navMeshAgent)
    {
        _transform = transform;
        //_animator = transform.GetComponent<Animator>();
        _rigid = rigid;
        _speed = speed;
        _navMeshAgent = navMeshAgent;
        //Debug.Log("고투타겟 생성자 호출!");
    }

    public override NodeState Evaluate()
    {
        _navMeshAgent.speed = _speed;

        if(_navMeshAgent.isStopped)
            _navMeshAgent.isStopped = false;

        //Debug.Log("GoToTarget  " + _navMeshAgent.speed);

        Transform target = (Transform)GetData("target");
        Vector3 directionToTarget = (target.position - _transform.position).normalized;

        //_animator.SetBool("Walking", false);
        //_animator.SetBool("Waiting", false);
        //_animator.SetBool("Running", true);

        if (Vector3.Distance(_transform.position, target.position) > 0.01f)
        {
            //Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            //Quaternion _smoothRotation = Quaternion.Lerp(_transform.rotation, targetRotation, 0.07f);
            //_rigid.MoveRotation(_smoothRotation);


            //_rigid.MovePosition(_transform.position + (_transform.forward * _speed * Time.deltaTime));
            

            _navMeshAgent.SetDestination(target.position);

            if (Vector3.Distance(_transform.position, target.position) < 11f)
                _navMeshAgent.isStopped = true;

            //if (_navMeshAgent == null)
            //{
            //    Debug.Log("NavMeshAgent is not assigned or is invalid!");
            //}
            //else if (!_navMeshAgent.isOnNavMesh)
            //{
            //    Debug.Log("NavMeshAgent is not placed on a NavMesh!");
            //}

        }

        state = NodeState.RUNNING;
        return state;
    }
}