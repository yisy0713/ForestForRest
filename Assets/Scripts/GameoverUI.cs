using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverUI : MonoBehaviour
{
    [SerializeField]
    private GameObject go_GameoverUI;

    private StatusUI playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = FindObjectOfType<StatusUI>();
        go_GameoverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStatus.GetIsDead())
            GameOverUIActivate();
    }

    public void GameOverUIActivate()
    {
        go_GameoverUI.SetActive(true);
    }
}
