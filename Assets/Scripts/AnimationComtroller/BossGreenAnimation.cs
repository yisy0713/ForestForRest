using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGreenAnimation : MonoBehaviour
{
    private Animator _animator;
    private BossAI _bossAI;

    public GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _bossAI = GetComponent<BossAI>();

    }

    // Update is called once per frame
    void Update()
    {
    }
}
