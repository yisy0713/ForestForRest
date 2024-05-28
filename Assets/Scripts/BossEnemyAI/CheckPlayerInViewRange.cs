using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckPlayerInViewRange : Node
{
    private static int _playerLayerMask = 1 << 8;

    private Transform _transform;
    private float _distance;

    public CheckPlayerInViewRange(Transform transform, float distance)
    {
        _transform = transform;
        _distance = distance;
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;

        for (int i = 0; i < 10; i++)
        {
            Vector3 startPosition = new Vector3(_transform.position.x, _transform.position.y - 3 + i, _transform.position.z);
            Vector3 forwardDirection = _transform.forward * _distance;

            // ����� ���� �׸���
            Debug.DrawRay(startPosition, forwardDirection, Color.green);

            // ����ĳ��Ʈ ����
            if (Physics.Raycast(startPosition, _transform.forward, out hit, _distance, _playerLayerMask))
            {
                // ����ĳ��Ʈ�� �÷��̾ �ε����� ���鿡 �÷��̾ �ִٰ� �Ǵ�
                if (hit.collider.CompareTag("Player"))
                {
                    //Debug.Log("���鿡 �÷��̾� ����!");
                    state = NodeState.SUCCESS;
                    return state;
                }
            }
        }

        //Debug.Log("���� �÷��̾� ����");
        state = NodeState.FAILURE;
        return state;
    }

}
