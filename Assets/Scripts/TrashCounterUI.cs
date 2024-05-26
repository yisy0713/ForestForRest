using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TrashCounterUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI trashCountText;
    [SerializeField]
    private TextMeshProUGUI trashTotalCountText;
    [SerializeField]
    private Inventory theInventory;
    [SerializeField]
    private EnvironmentManager theEnvironment;
    [SerializeField]
    private Item trashItem;

    public int GoalTrashCount;

    public int currTrashCount;
    public int totalTrashCount;

    // Start is called before the first frame update
    void Start()
    {
        GoalTrashCount = 100;
        currTrashCount = 0;
        trashCountText.text = currTrashCount.ToString();
        totalTrashCount = 0;
        trashTotalCountText.text = currTrashCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        trashCounter();
        totalTrashCounter();
        if (totalTrashCount >= GoalTrashCount)
            theEnvironment.DoneCollectTrash = true;
    }

    private void trashCounter()
    {
        currTrashCount = theInventory.CountItem(trashItem);
        trashCountText.text = currTrashCount.ToString();
    }

    private void totalTrashCounter()
    {
        totalTrashCount = theInventory.CountTotalItem(trashItem);
        trashTotalCountText.text = totalTrashCount.ToString() + "/" + GoalTrashCount;
    }
}
