using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGreenAnimation : MonoBehaviour
{
    private Animator _animator;
    private BossAI _bossAI;

    public GameObject _player;

    private float walkspeed = 5f;
    private float runSpeed = 5f;
    private float flySpeed = 30f;
    private float nearAttackRange = 11f;
    private float farAttackRange = 15f;
    private float nearFovRange = 30f;
    private float farFovRange = 60f;
    private float fleeFovRange = 80f;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _bossAI = GetComponent<BossAI>();

        walkspeed = _bossAI.walkspeed;
        runSpeed = _bossAI.runSpeed;
        flySpeed = _bossAI.flySpeed;
        //nearAttackRange = _bossAI.nearAttackRange;
        farAttackRange = _bossAI.farAttackRange;
        nearFovRange = _bossAI.nearFovRange;
        farFovRange = _bossAI.farFovRange;
        fleeFovRange = _bossAI.fleeFovRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < nearAttackRange)
            _animator.SetTrigger("Fireball");
    }
}
