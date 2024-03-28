using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField]
    private Hand currentHand;

    [SerializeField]
    private PlayerController playerController;

    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TryAttack();

        AnimationCntroller();
    }

    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
                if (hitInfo.transform.CompareTag("Enemy") || hitInfo.transform.CompareTag("SpawnedEnemy"))
                {
                    hitInfo.transform.GetComponent<EnemyManager>().EnemyDecreaseHp(currentHand.damage);
                }
            }
            yield return null;
        }
    }

    private bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }

    private void AnimationCntroller()
    {
        if (!playerController.isStop)
        {
            if (playerController.isRun)
            {
                currentHand.anim.SetBool("Running", true);
            }
            else
            {
                currentHand.anim.SetBool("Running", false);
                currentHand.anim.SetBool("Walking", true);
            }
        }
        else
        {
            currentHand.anim.SetBool("Walking", false);
            currentHand.anim.SetBool("Running", false);
        }
    }
}
