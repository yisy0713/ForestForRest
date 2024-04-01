using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar;

    [SerializeField]
    private Slider hungryBar;

    [SerializeField]
    private Slider spBar;


    // 체력 Hp
    public float maxHp = 100f;
    public float curHp = 100f;
    public float hpIncreaseSpeed = 1f;           // 체력 증가량
    public float hpRechargeTime = 500f;          // 체력 회복 딜레이 500
    public float curHpRechargeTime = 0f;
    private bool hpUpTime = false;

    // 배고픔 Hungry
    public float maxHungry = 100f;
    public float curHungry = 100f;
    public float hungryDecreaseTime = 30;       // 배고픔 감소 쿨타임
    public float curHungryDecreaseTime = 0;     // 배고픔 감소 타이머
    private bool imNotHungry = true;

    // 스테미나 Sp
    private float maxSp = 100f;
    private float curSp = 100f;
    public float spIncreaseSpeed = 20f;        // 스테미나 증가량
    public float spRechargeTime = 3;           // 스테미나 회복 딜레이
    public float curSpRechargeTime;
    private bool spUsed;                  // 스테미나 감소 여부 (true : 회복딜레이 / false : 회복시작)

    // 방어력 Dp
    private float dp;
    private float curDp;

    float hp;
    float hungry;
    float sp;

    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        hp = curHp / maxHp;
        hpBar.value = hp;

        hungry = curHungry / maxHungry;
        hungryBar.value = hungry;

        sp = curSp / maxSp;
        spBar.value = sp;

        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();

        HpRechargeTime();
        HpRecover();

        SpRechargeTime();
        SpRecover();

        UpdateUIBar();
    }

    public void IncreaseHp(float count)               // 체력 증가 함수
    {
        if (curHp + count < maxHp)
            curHp += count;
        else
            curHp = maxHp;

        hp = curHp / maxHp;
    }
    
    public void DecreaseHp(float count)               // 체력 감소 함수
    {

        if (curHp > 0)
        {
            curHp -= count;
        }
        else
        {
            isDead = true;
        }

        hp = curHp / maxHp;

    }

    private void HpRechargeTime()                  // 체력 회복 쿨타임 관리
    {
        if (curHpRechargeTime < hpRechargeTime)
            curHpRechargeTime++;
        else
            hpUpTime = true;
    }

    private void HpRecover()                        // 체력 회복
    {
        if (hpUpTime && !isDead && (curHp < maxHp) && imNotHungry)
        {
            if (curHp + hpIncreaseSpeed < maxHp)
                curHp += hpIncreaseSpeed;
            else
                curHp = maxHp;

            hpUpTime = false;
            curHpRechargeTime = 0;
        }
    }

    private void Hungry()
    {
        if(curHungry > 0)       // 배고픔이 0보다 클때
        {
            imNotHungry = true;

            curHungryDecreaseTime += Time.deltaTime;

            if (curHungryDecreaseTime >= hungryDecreaseTime)
            {
                curHungry--;
                curHungryDecreaseTime = 0;
            }
        }
        else // 배고픔이 0일때
        {
            imNotHungry = false;
        }

        hungry = curHungry / maxHungry;
    }         

    public void IncreaseHungry(int count)           // 배고픔 증가 함수
    {
        if (curHungry + count < maxHungry)
            curHungry += count;
        else
            curHungry = maxHungry;
    }

    public void DecreaseStamina(float count)
    {
        spUsed = true;
        curSpRechargeTime = 0;

        float decreaseAmount = count * Time.deltaTime;

        if (curSp - decreaseAmount > 0)
            curSp -= decreaseAmount;
        else
            curSp = 0;
    }

    private void SpRechargeTime()                  // 스테미나 회복 쿨타임 관리
    {
        curSpRechargeTime += Time.deltaTime;

        if (curSpRechargeTime >= spRechargeTime)
        {
            spUsed = false;
            curSpRechargeTime = 0f;
        }
    }

    private void SpRecover()                        // 스테미나 회복
    {
        if(!spUsed && (curSp < maxSp) && imNotHungry)
        {
            curSp += spIncreaseSpeed * Time.deltaTime;
        }
    }

    public float GetCurSp()
    {
        return curSp;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    private void UpdateUIBar()
    {
        hp = curHp / maxHp;
        sp = curSp / maxSp;

        hpBar.value = Mathf.Lerp(hpBar.value, hp, Time.deltaTime * 10);                   // 게이지가 부드럽게 감소or증가
        hungryBar.value = Mathf.Lerp(hungryBar.value, hungry, Time.deltaTime * 1);
        spBar.value = Mathf.Lerp(spBar.value, sp, Time.deltaTime * 10);
    }

    // 패시브 아이템 획득시 호출되는 함수

    public void IncreaseMaxHp(float count)
    {
        maxHp += count;
        curHp += count;
    }

    public void IncreaseHpRecover(float count)
    {
        hpIncreaseSpeed += count;
    }

    public void IncreaseHungerDecreaseDelay(float count)
    {
        hungryDecreaseTime += count;
    }

    public void IncreaseSpRecover(float count)
    {
        if(spIncreaseSpeed + count < 40)
        {
            spIncreaseSpeed += count;
        }
        else
        {
            spIncreaseSpeed = 40f;
        }
        
    }
}

