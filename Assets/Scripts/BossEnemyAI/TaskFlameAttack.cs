using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskFlameAttack : Node
{
    private StatusUI _playerStatus;
    private Transform _transform;
    private GameObject _poisionFlame;
    private float _damage;

    public TaskFlameAttack(Transform transform, GameObject poisionFlame, float damage)
    {
        _playerStatus = Object.FindObjectOfType<StatusUI>();
        _transform = transform;
        _poisionFlame = poisionFlame;
        _damage = damage;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("FlameAttack");

        Vector3 FirePosition = new Vector3(_transform.position.x, _transform.position.y + 3f, _transform.position.z);
        GameObject instantPoisionBall = Object.Instantiate(_poisionFlame, FirePosition, _transform.rotation);

        _playerStatus.DecreaseHp(_damage);

        state = NodeState.RUNNING;
        return state;
    }
}
