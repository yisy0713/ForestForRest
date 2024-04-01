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
    //public static bool stop = false;

    private bool isNight = true;

    [SerializeField]
    private GameObject directionalLight;

    public float oneDayCycle;               // �Ϸ� �ֱ� (�� ����)

    [SerializeField]
    private GameObject timer;
    private TextMeshProUGUI timerText;

    [SerializeField]
    private GameObject dayCounter;
    private TextMeshProUGUI dayCounterText;

    [SerializeField]
    private GameObject monsterPrefab;

    [SerializeField]
    private float spawnRadius = 10f; // ���� �ݰ�
    [SerializeField]
    private float spawnInterval = 10f; // ���� ����
    private float nextSpawnTime = 0f;

    [SerializeField]
    private Transform player;

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
        time += Time.deltaTime;
        directionalLight.transform.Rotate(Vector3.up, 3 * Time.deltaTime / oneDayCycle);
        currentTime = (int)time;

        currentDayCount = (int)(currentTime / 60);

        //if((int)(currentTime / 30))

        if ((currentTime - (60 * currentDayCount) < 30) && isNight) // ���� �Ǿ��� ��
        {
            isNight = false;
            Debug.Log("Day");
            // ���⿡ ���� �ʿ��� �۾� �߰�
        }
        else if ((currentTime - (60 * currentDayCount) >= 30) && !isNight) // ���� �Ǿ��� ��
        {
            isNight = true;
            Debug.Log("Night");
            // ���⿡ �㿡 �ʿ��� �۾� �߰�
            SpawnMonsters();
        }

        

        UpdateUI();
    }
    void SpawnMonsters()
    {
        if (Time.time >= nextSpawnTime)
        {
            // �÷��̾� �ֺ��� ���� ����
            Vector3 spawnOffset = Random.insideUnitSphere * spawnRadius;
            Vector3 spawnPosition = player.position + spawnOffset;
            spawnPosition.y = 0;
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void UpdateUI()
    {
        timerText.text = "Time : " + currentTime.ToString();
        dayCounterText.text = currentDayCount.ToString() + " Day";
    }

}
