using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TrashCounterUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI trashCountText;
    [SerializeField]
    private Inventory theInventory;
    [SerializeField]
    private Item trashItem;

    public int trashCount;

    // Start is called before the first frame update
    void Start()
    {
        trashCount = 0;
        trashCountText.text = trashCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        trashCounter();
    }

    private void trashCounter()
    {
        trashCount = theInventory.CountItem(trashItem);
        trashCountText.text = trashCount.ToString();
    }
}
