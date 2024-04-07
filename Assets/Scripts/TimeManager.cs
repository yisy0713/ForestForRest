using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    private float time;
    private int currentTime;

    private int currentDayCount = 0;
    private bool checkedDayCount = false;

    //private bool isNight = true;

    [SerializeField]
    private GameObject directionalLight;

    [SerializeField]
    private GameObject timer;
    private TextMeshProUGUI timerText;

    [SerializeField]
    private GameObject dayCounter;
    private TextMeshProUGUI dayCounterText;

    [SerializeField]
    private GameObject monsterPrefab;

    [SerializeField]
    private float spawnRadius = 10f;        // ���� ���� �ݰ�
    [SerializeField]
    private float spawnInterval = 3f;       // ���� ���� �ð� ����
    private float EnemySpawnTimer = 0f;     // ���� ���� Ÿ�̸�
    


    [SerializeField]
    private Transform player;

    

    [SerializeField]
    private float secondPerRealTimeSecond = 100;

    private bool isNight = false;

    [SerializeField]
    private float fogDensityCale;
    [SerializeField]
    private float nightFogDensity;
    private float dayFogDensity = 0f;
    private float currentFogDensity;

    // Start is called before the first frame update
    void Start()
    {
        timerText = timer.GetComponent<TextMeshProUGUI>();
        dayCounterText = dayCounter.GetComponent<TextMeshProUGUI>();
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        directionalLight.transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if(directionalLight.transform.eulerAngles.x >= 170)
        {
            isNight = true;
        }
        else if(directionalLight.transform.eulerAngles.x <= 1)
        {
            isNight = false;
        }

        if (isNight)    // �� �϶�
        {
            checkedDayCount = true;

            SpawnMonsters();
        }
        else            // �� �϶�
        {
            if (checkedDayCount)    // ���� �Ǹ� ���� �ϼ� ������Ʈ
            {
                currentDayCount++;
                checkedDayCount = false;
            }
        }
        

        UpdateFogDensity();

        UpdateUI();
    }

    private void UpdateFogDensity()
    {
        if (isNight)
        {
            Debug.Log("��");
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCale * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            Debug.Log("��");
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.5f * fogDensityCale * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }

    void SpawnMonsters()
    {
        if(EnemySpawnTimer >= spawnInterval)
        {
            Vector3 spawnOffset = Random.insideUnitSphere * spawnRadius;
            Vector3 spawnPosition = player.position + spawnOffset;
            spawnPosition.y = player.position.y + 5;
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            EnemySpawnTimer = 0f;
        }
        else
        {
            EnemySpawnTimer += Time.deltaTime;
        }
    }

    void UpdateUI()
    {
        //timerText.text = "Time : " + currentTime.ToString();
        dayCounterText.text = currentDayCount.ToString() + " Day";
    }

}
