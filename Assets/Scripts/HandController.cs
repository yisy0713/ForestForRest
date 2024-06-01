using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField]
    private Hand currentHand;

    private PlayerController player;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Hand[] Weapons;

    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;

        player = FindObjectOfType<PlayerController>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(2);
        }


        TryAttack();

        AnimationCntroller();
    }

    private void EquipWeapon(int _index)
    {
        if(currentHand == Weapons[_index])
        {
            EquipWeapon(0);
        }
        else
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (i == _index)
                {
                    Weapons[i].gameObject.SetActive(true);
                    currentHand = Weapons[i];
                    continue;
                }

                Weapons[i].gameObject.SetActive(false);
            }
        }
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
                    hitInfo.transform.GetComponent<EnemyManager>().EnemyDecreaseHp(currentHand.damage + player.attackDamage);             // 데미지 부분
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

    public void HandChange(Hand _hand)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentHand = _hand;
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero;
        currentHand.gameObject.SetActive(true);
    }
}
