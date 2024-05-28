using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public float maxHp = 100f;
    public float curHp = 100f;

    public float hp;

    public bool enemyDead = false;

    public bool isBoss = false;

    [SerializeField]
    private Slider BossHPBar;

    private Animator _animator;
    private Transform _transform;

    [SerializeField]
    private GameObject[] dropItem;

    EnvironmentManager environmentManager;

    // Start is called before the first frame update
    void Start()
    {
        hp = curHp / maxHp;
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        environmentManager = FindObjectOfType<EnvironmentManager>();

        if (isBoss)
        {
            maxHp = 300f;
            curHp = 300f;
            BossHPBar.value = hp;
            //BossHPBar.gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyDead)
        {
            StartCoroutine(DisableObjectAfterDelay(2f));
        }

        //if (isBoss)
        //{
        //    hp = curHp / maxHp;
        //    BossHPBar.value = Mathf.Lerp(BossHPBar.value, hp, Time.deltaTime * 10);
        //}
    }

    public void EnemyDecreaseHp(float count)
    {
        if (curHp > 0)
        {
            curHp -= count;

            if (!isBoss)
            {
                _animator.SetTrigger("Hit");
            }
                
            if (curHp <= 0)
            {
                EnemyDead();
                enemyDead = true;
            }
        }
        
        hp = curHp / maxHp;
        EnemyHpBar.Instance.UpdateEnemyHP(this);
        Debug.Log("현재 적 체력 : " + hp);
    }

    public void EnemyIncreaseHp(float count)
    {
        curHp += count;
        if (curHp > maxHp)
        {
            curHp = maxHp;
        }

        hp = curHp / maxHp;
        //Debug.Log("현재 적 체력 : " + curHp);
    }

    private void EnemyDead()
    {
        _animator.SetTrigger("Dead");

        if (isBoss)
        {
            //BossHPBar.gameObject.SetActive(false);
        }

        //Instantiate(dropItem[0], _transform.position, Quaternion.identity);
    }

    IEnumerator DisableObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        DropItem();

        Destroy(gameObject);
    }

    private void DropItem()
    {
        int numOfDropItem = Random.Range(1, 3);     // 몬스터 처치시 전리품 1~3개 드랍
        for (int i = 0; i < numOfDropItem; i++)
        {
            GameObject obj = dropItem[Random.Range(0, dropItem.Length)];
            Vector3 dropPos = new Vector3(_transform.position.x, _transform.position.y + 1, _transform.position.z);//_transform.position;

            Instantiate(obj, dropPos, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        EnemyHpBar.Instance.HideHPUI();
        if (isBoss)
        {
            environmentManager.CleanUp();
        }
    }
}
