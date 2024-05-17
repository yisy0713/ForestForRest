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
    private float attackRange = 1.2f;

    public static float timer = 0f;

    private NavMeshAgent navMeshAgent;
    private void Awake()
    {
        navMeshAgent = transform.GetComponent<NavMeshAgent>();
    }
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new CheckAmIDead(transform),
            new Sequence(new List<Node>
            {
                new CheckPlayerInRange(transform, attackRange),
                new TaskAttack(transform),
            }),
            new Sequence(new List<Node>
            {
                new CheckPlayerInRange(transform, fovRange),
                new TaskGoToTarget(transform, rigid, runSpeed, navMeshAgent),
            }),
            new TaskPatrol(transform, rigid, walkSpeed, navMeshAgent)
        });

        return root;
    }
}
