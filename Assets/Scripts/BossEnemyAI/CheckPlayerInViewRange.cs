using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckPlayerInViewRange : Node
{
    private static int _playerLayerMask = 1 << 8;

    private Transform _transform;

    public CheckPlayerInViewRange(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;

        for (int i = 0; i < 10; i++)
        {
            Vector3 startPosition = new Vector3(_transform.position.x, _transform.position.y - 3 + i, _transform.position.z);
            Vector3 forwardDirection = _transform.forward * 10f;

            // 디버그 레이 그리기
            Debug.DrawRay(startPosition, forwardDirection, Color.green);

            // 레이캐스트 실행
            if (Physics.Raycast(startPosition, _transform.forward, out hit, 10f, _playerLayerMask))
            {
                // 레이캐스트가 플레이어에 부딪히면 정면에 플레이어가 있다고 판단
                if (hit.collider.CompareTag("Player"))
                {
                    //Debug.Log("정면에 플레이어 있음!");
                    state = NodeState.SUCCESS;
                    return state;
                }
            }
        }

        //Debug.Log("정면 플레이어 없음");
        state = NodeState.FAILURE;
        return state;
    }

}
