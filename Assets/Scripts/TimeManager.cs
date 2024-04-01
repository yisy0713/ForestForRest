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

    public float oneDayCycle;               // 하루 주기 (분 단위)

    [SerializeField]
    private GameObject timer;
    private TextMeshProUGUI timerText;

    [SerializeField]
    private GameObject dayCounter;
    private TextMeshProUGUI dayCounterText;

    [SerializeField]
    private GameObject monsterPrefab;

    [SerializeField]
    private float spawnRadius = 10f; // 스폰 반경
    [SerializeField]
    private float spawnInterval = 10f; // 스폰 간격
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

        if ((currentTime - (60 * currentDayCount) < 30) && isNight) // 낮이 되었을 때
        {
            isNight = false;
            Debug.Log("Day");
            // 여기에 낮에 필요한 작업 추가
        }
        else if ((currentTime - (60 * currentDayCount) >= 30) && !isNight) // 밤이 되었을 때
        {
            isNight = true;
            Debug.Log("Night");
            // 여기에 밤에 필요한 작업 추가
            SpawnMonsters();
        }

        

        UpdateUI();
    }
    void SpawnMonsters()
    {
        if (Time.time >= nextSpawnTime)
        {
            // 플레이어 주변에 몬스터 스폰
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
