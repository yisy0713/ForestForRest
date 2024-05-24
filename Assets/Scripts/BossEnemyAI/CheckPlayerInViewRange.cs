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

            // ����� ���� �׸���
            Debug.DrawRay(startPosition, forwardDirection, Color.green);

            // ����ĳ��Ʈ ����
            if (Physics.Raycast(startPosition, _transform.forward, out hit, 10f, _playerLayerMask))
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
