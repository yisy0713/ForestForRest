using System.Collections.Generic;
using BehaviorTree;

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


    public static float timer = 0f;
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
                            new TaskGoToTarget(transform, rigid, runSpeed)
                        }),
                        new Sequence(new List<Node>
                        {
                            new CheckPlayerInRange(transform, farFovRange),
                            new TaskGoToTarget(transform, rigid, flySpeed)
                        }),
                        new TaskPatrol(transform, rigid, walkspeed)
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
                            new TaskGoToTarget(transform, rigid, flySpeed)
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
/// µð¹ö±×¿ë ³ëµåµé
/// </summary>

public class CheckRandom1 : Node
{
    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("·£´ý 1111111111111");

        state = NodeState.SUCCESS;
        return state;
    }
}
public class CheckRandom2 : Node
{
    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("·£´ý 22222222222222");

        state = NodeState.SUCCESS;
        return state;
    }
}
public class CheckRandom3 : Node
{
    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("·£´ý 333333333333333");

        state = NodeState.SUCCESS;
        return state;
    }
}
public class CheckRandom : Node
{
    public override NodeState Evaluate()
    {
        UnityEngine.Debug.Log("·£´ý----------------------");

        state = NodeState.FAILURE;
        return state;
    }
}