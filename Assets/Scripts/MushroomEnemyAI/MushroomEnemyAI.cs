using System.Collections.Generic;
using BehaviorTree;
using UnityEngine.AI;

public class MushroomEnemyAI : Tree
{
    public UnityEngine.Transform[] wayPoints;
    public UnityEngine.Transform returnPoint;
    public UnityEngine.Rigidbody rigid;

    public static float walkSpeed = 1.5f;
    public static float runSpeed = 3f;
    private float fovRange = 8f;
    private float attackRange = 2f;
    private float attackCoolTime = 1f;
    private float attackDamage = 15f;

    public static float timer = 0f;

    private NavMeshAgent navMeshAgent;
    private void Awake()
    {
        UnityEngine.Vector3 position = transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, UnityEngine.Mathf.Infinity, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
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
                new CheckPlayerInRange(transform, attackRange, true),
                new Selector(new List<Node>
                {
                    new CheckPlayerInViewRange(transform, 5f),
                    new Sequence(new List<Node>
                    {
                        new SetAnim(transform, "Walk"),
                        new TaskLookPlayer(transform)
                    })
                }),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new CheckAttackTimer(attackCoolTime),
                        new Sequence(new List<Node>
                        {
                            new SetAnim(transform, "Attack"),
                            new TaskAttack(transform, attackDamage),
                        })
                    }),
                    new Sequence(new List<Node>
                    {
                        new SetAnim(transform, "Idle"),
                        new TaskWaitAttack(transform),
                    }),
                }),
                
            }),
            new Sequence(new List<Node>
            {
                new CheckPlayerInRange(transform, fovRange),
                new SetAnim(transform, "Run"),
                new TaskGoToTarget(transform, runSpeed, navMeshAgent),
            }),
            new TaskPatrol(transform, rigid, walkSpeed, navMeshAgent)
        });

        return root;
    }
}
