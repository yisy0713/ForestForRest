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

    private bool isNight = false;

    [SerializeField]
    private GameObject directionalLight;

    public float oneDayCycle;               // 하루 주기 (분 단위)

    [SerializeField]
    private GameObject timer;
    private TextMeshProUGUI timerText;

    [SerializeField]
    private GameObject dayCounter;
    private TextMeshProUGUI dayCounterText;

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

        UpdateUI();
    }

    void UpdateUI()
    {
        timerText.text = "Time : " + currentTime.ToString();
        dayCounterText.text = currentDayCount.ToString() + " Day";
    }

}
