using System.Collections.Generic;
using BehaviorTree;
using UnityEngine.AI;

public class BossAI : Tree
{
    public UnityEngine.Rigidbody rigid;

    private UnityEngine.Animator animator;

    private float walkspeed = 5f;
    private float runSpeed = 10f;
    private float flySpeed = 30f;
    private float nearAttackRange = 10f;
    private float farAttackRange = 15f;
    private float nearFovRange = 30f;
    private float farFovRange = 60f;
    private float fleeFovRange = 80f;
    private float attackTimer = 3f;

    public UnityEngine.GameObject poisionBall;
    public UnityEngine.GameObject poisionFlame;

    public EnemyManager enemyManager;

    public NavMeshAgent navMeshAgent;

    public static float timer = 0f;

    private void Awake()
    {
        animator = GetComponent<UnityEngine.Animator>();

        // NavMesh 상의 유효한 위치 찾기
        UnityEngine.Vector3 position = transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, UnityEngine.Mathf.Infinity, NavMesh.AllAreas))
        {
            // 유효한 위치를 NavMeshAgent의 초기 위치로 설정
            transform.position = hit.position;
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            UnityEngine.Debug.Log("Add 네비메쉬에이전트! (BOSSAI Awake함수!)");
            navMeshAgent.radius = 1f;
            navMeshAgent.height = 3f;
            navMeshAgent.Warp(hit.position);
        }

    }

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new CheckAmIDead(transform),

            new Sequence(new List<Node>
            {
                new CheckHpEnough(transform, 200),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new CheckPlayerInRange(transform, nearAttackRange , true),
                        new SetAnim(transform, "Idle"),
                        new Selector(new List<Node>
                        {
                            new CheckPlayerInViewRange(transform, 10f),
                            new Sequence(new List<Node>
                            {
                                new SetAnim(transform, "Walk"),
                                new TaskLookPlayer(transform),
                            }),
                        }),
                        new Selector(new List<Node>
                        {
                            new Sequence(new List<Node>
                            {
                                new CheckAttackTimer(attackTimer),
                                new RandomSelector(new List<Node>
                                {
                                    new Sequence(new List<Node>
                                    {
                                        new SetAnim(transform, "Tail"),
                                        new TaskTailAttack(transform, 30)
                                    }),
                                    new Sequence(new List<Node>
                                    {
                                        new SetAnim(transform, "Scream"),
                                        new TaskFlameAttack(transform, poisionFlame, 30)
                                    }),
                                    new Sequence(new List<Node>
                                    {
                                        new SetAnim(transform, "Fireball"),
                                        new TaskFireBallAttack(transform, poisionBall)
                                    }),
                                    new Sequence(new List<Node>
                                    {
                                        new SetAnim(transform, "Bite"),
                                        new TaskBiteAttack(transform, 30)
                                    }),
                                })
                            }),
                            new Sequence(new List<Node>
                            {
                                new SetAnim(transform, "Idle"),
                                new TaskWaitAttack(transform)
                            })
                        })
                    }),
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new CheckPlayerInRange(transform, nearFovRange),
                            new SetAnim(transform, "Run"),
                            new TaskGoToTarget(transform, runSpeed, navMeshAgent),
                        }),
                        new Sequence(new List<Node>
                        {
                            new CheckPlayerInRange(transform, farFovRange),
                            new SetAnim(transform, "Fly"),
                            new TaskGoToTarget(transform, flySpeed, navMeshAgent)
                        }),
                        new TaskPatrol(transform, rigid, walkspeed, navMeshAgent)
                    })
                })
            }),
            new Sequence(new List<Node>
            {
                new CheckHpEnough(transform,100),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new CheckOnGround(transform),
                        new Sequence(new List<Node>
                        {
                            new SetAnim(transform, "Fly"),
                            new TaskFlyTakeOff(transform),
                        }),
                        
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckPlayerInRange(transform, farAttackRange, true),
                        new Selector(new List<Node>
                        {
                            new Sequence(new List<Node>
                            {
                                new CheckAttackTimer(attackTimer),
                                new SetAnim(transform, "Flyfireball"),
                                new TaskFlyFireBallAttack(transform, poisionBall)
                            }),
                            new SetAnim(transform, "Flying"),
                            new TaskWaitAttack(transform)
                        })
                    }),
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new CheckPlayerInRange(transform, farFovRange),
                            new SetAnim(transform, "Flying"),
                            new TaskGoToTarget(transform, flySpeed, navMeshAgent)
                        }),
                        new Sequence(new List<Node>
                        {
                            new SetAnim(transform, "Sleep"),
                            new TaskFlyLand(transform),
                            new TaskHeal(transform)
                        }),
                        
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
/// 디버그용 노드들
/// </summary>

public class CheckRandom1 : Node
{
    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("랜덤 1111111111111");

        state = NodeState.SUCCESS;
        return state;
    }
}
public class CheckRandom2 : Node
{
    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("랜덤 22222222222222");

        state = NodeState.SUCCESS;
        return state;
    }
}
public class CheckRandom3 : Node
{
    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("랜덤 333333333333333");

        state = NodeState.SUCCESS;
        return state;
    }
}
public class CheckRandom : Node
{
    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("랜덤----------------------");

        state = NodeState.FAILURE;
        return state;
    }
}