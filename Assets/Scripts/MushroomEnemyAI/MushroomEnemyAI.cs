using System.Collections.Generic;
using BehaviorTree;

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
                new TaskGoToTarget(transform, rigid, runSpeed),
            }),
            new TaskPatrol(transform, rigid, walkSpeed)
        });

        return root;
    }
}
