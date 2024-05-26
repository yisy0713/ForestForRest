using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    Canvas canvas;

    [SerializeField]
    private Slider hpBar;

    public EnemyManager enemyManager;

    public Camera mainCamera;

    // Start is called before the first frame update
    private void Start()
    {
        canvas = GetComponent<Canvas>();

        mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
        //canvas.worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        canvas.worldCamera = mainCamera;
    }

    private void Update()
    {
        UpdateUIBar();
    }

    private void UpdateUIBar()
    {
        enemyManager.hp = enemyManager.curHp / enemyManager.maxHp;

        hpBar.value = Mathf.Lerp(hpBar.value, enemyManager.hp, Time.deltaTime * 10);
    }
}
