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


    // ü�� Hp
    public float maxHp = 100f;
    public float curHp = 100f;
    public float hpIncreaseSpeed = 1f;           // ü�� ������
    public float hpRechargeTime = 500f;          // ü�� ȸ�� ������ 500
    public float curHpRechargeTime = 0f;
    private bool hpUpTime = false;

    // ����� Hungry
    public float maxHungry = 100f;
    public float curHungry = 100f;
    public float hungryDecreaseTime = 30;       // ����� ���� ��Ÿ��
    public float curHungryDecreaseTime = 0;     // ����� ���� Ÿ�̸�
    private bool imNotHungry = true;

    // ���׹̳� Sp
    private float maxSp = 100f;
    private float curSp = 100f;
    public float spIncreaseSpeed = 20f;        // ���׹̳� ������
    public float spRechargeTime = 3;           // ���׹̳� ȸ�� ������
    public float curSpRechargeTime;
    private bool spUsed;                  // ���׹̳� ���� ���� (true : ȸ�������� / false : ȸ������)

    // ���� Dp
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

    public void IncreaseHp(float count)               // ü�� ���� �Լ�
    {
        if (curHp + count < maxHp)
            curHp += count;
        else
            curHp = maxHp;

        hp = curHp / maxHp;
    }
    
    public void DecreaseHp(float count)               // ü�� ���� �Լ�
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

    private void HpRechargeTime()                  // ü�� ȸ�� ��Ÿ�� ����
    {
        if (curHpRechargeTime < hpRechargeTime)
            curHpRechargeTime++;
        else
            hpUpTime = true;
    }

    private void HpRecover()                        // ü�� ȸ��
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
        if(curHungry > 0)       // ������� 0���� Ŭ��
        {
            imNotHungry = true;

            curHungryDecreaseTime += Time.deltaTime;

            if (curHungryDecreaseTime >= hungryDecreaseTime)
            {
                curHungry--;
                curHungryDecreaseTime = 0;
            }
        }
        else // ������� 0�϶�
        {
            imNotHungry = false;
        }

        hungry = curHungry / maxHungry;
    }         

    public void IncreaseHungry(int count)           // ����� ���� �Լ�
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

    private void SpRechargeTime()                  // ���׹̳� ȸ�� ��Ÿ�� ����
    {
        curSpRechargeTime += Time.deltaTime;

        if (curSpRechargeTime >= spRechargeTime)
        {
            spUsed = false;
            curSpRechargeTime = 0f;
        }
    }

    private void SpRecover()                        // ���׹̳� ȸ��
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

        hpBar.value = Mathf.Lerp(hpBar.value, hp, Time.deltaTime * 10);                   // �������� �ε巴�� ����or����
        hungryBar.value = Mathf.Lerp(hungryBar.value, hungry, Time.deltaTime * 1);
        spBar.value = Mathf.Lerp(spBar.value, sp, Time.deltaTime * 10);
    }

    // �нú� ������ ȹ��� ȣ��Ǵ� �Լ�

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

