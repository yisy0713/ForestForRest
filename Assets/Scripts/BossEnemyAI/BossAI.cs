using System.Collections.Generic;
using BehaviorTree;
using UnityEngine.AI;

public class BossAI : Tree
{
    public UnityEngine.Rigidbody rigid;

    public static float walkspeed = 5f;
    public static float runSpeed = 15f;
    public static float flySpeed = 30f;
    private float nearAttackRange = 10f;
    private float farAttackRange = 15f;
    private float nearFovRange = 30f;
    private float farFovRange = 60f;
    private float fleeFovRange = 80f;
    private float attackTimer = 2f;

    public EnemyManager enemyManager;

    public NavMeshAgent navMeshAgent;

    public static float timer = 0f;

    private void Awake()
    {
        // NavMesh ���� ��ȿ�� ��ġ ã��
        UnityEngine.Vector3 position = transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, UnityEngine.Mathf.Infinity, NavMesh.AllAreas))
        {
            // ��ȿ�� ��ġ�� NavMeshAgent�� �ʱ� ��ġ�� ����
            transform.position = hit.position;
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            UnityEngine.Debug.Log("Add �׺�޽�������Ʈ! (BOSSAI Awake�Լ�!)");
            navMeshAgent.Warp(hit.position);
        }
        else
        {
            UnityEngine.Debug.LogError("No valid NavMesh location found for setting initial position!");
        }

        if (navMeshAgent == null)
        {
            UnityEngine.Debug.Log("NavMeshAgent is not assigned or is invalid!!!!!!!!!!!!!");
        }

    }

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new CheckAmIDead(transform),

            new Sequence(new List<Node>
            {
                new CheckHpEnough(transform, 50),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new CheckPlayerInRange(transform, nearAttackRange),
                        new Selector(new List<Node>
                        {
                            new Sequence(new List<Node>
                            {
                                new CheckAttackTimer(attackTimer),
                                new RandomSelector(new List<Node>
                                {
                                    new TaskTailAttack(transform),
                                    new TaskFlameAttack(transform),
                                    new TaskFireBallAttack(transform),
                                    new TaskBiteAttack(transform)
                                })
                            }),
                            new TaskWaitAttack(transform)
                        })
                    }),
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new CheckPlayerInRange(transform, nearFovRange),
                            new TaskGoToTarget(transform, rigid, runSpeed, navMeshAgent)
                        }),
                        new Sequence(new List<Node>
                        {
                            new CheckPlayerInRange(transform, farFovRange),
                            new TaskGoToTarget(transform, rigid, flySpeed, navMeshAgent)
                        }),
                        new TaskPatrol(transform, rigid, walkspeed, navMeshAgent)
                    })
                })
            }),
            new Sequence(new List<Node>
            {
                new CheckHpEnough(transform, 20),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        // CheckOnGround
                        // TaskFlyTakeOff
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckPlayerInRange(transform, farAttackRange),
                        new Selector(new List<Node>
                        {
                            new Sequence(new List<Node>
                            {
                                // CheckAttackTimer
                                new RandomSelector(new List<Node>
                                {
                                    // TaskFireBallAttack
                                    // TaskGlideAttack
                                })
                            }),
                            new TaskWaitAttack(transform)
                        })
                    }),
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new CheckPlayerInRange(transform, farFovRange),
                            new TaskGoToTarget(transform, rigid, flySpeed, navMeshAgent)
                        }),
                        // TaskHeal
                    }),
                })
            }),
            new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckPlayerInRange(transform,fleeFovRange),
                    // TaskFlee
                }),
                // TaskHeal
            })
        });

        return root;
    }
}


/// <summary>
/// ����׿� ����
/// </summary>

public class CheckRandom1 : Node
{
    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("���� 1111111111111");

        state = NodeState.SUCCESS;
        return state;
    }
}
public class CheckRandom2 : Node
{
    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("���� 22222222222222");

        state = NodeState.SUCCESS;
        return state;
    }
}
public class CheckRandom3 : Node
{
    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("���� 333333333333333");

        state = NodeState.SUCCESS;
        return state;
    }
}
public class CheckRandom : Node
{
    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("����----------------------");

        state = NodeState.FAILURE;
        return state;
    }
}