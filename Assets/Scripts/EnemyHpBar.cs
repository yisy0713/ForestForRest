using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public static EnemyHpBar Instance;
    [SerializeField]
    private Slider enemyHp;
    private EnemyManager currEnemy;
    private float timer;
    public float displayDuration = 7f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (currEnemy != null)
        {
            timer += Time.deltaTime;
            if (timer >= displayDuration)
            {
                HideHPUI();
            }
            else
            {
                enemyHp.value = Mathf.Lerp(enemyHp.value, currEnemy.hp, Time.deltaTime * 10);
            }
        }
    }
    public void UpdateEnemyHP(EnemyManager enemy)
    {
        if (currEnemy != enemy)
        {
            currEnemy = enemy;
            enemyHp.value = currEnemy.hp;
        }

        timer = 0f;
        enemyHp.gameObject.SetActive(true);
    }

    public void HideHPUI()
    {
        currEnemy = null;
        enemyHp.gameObject.SetActive(false);
    }
}
