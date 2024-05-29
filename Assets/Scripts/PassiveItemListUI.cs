using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemListUI : MonoBehaviour
{
    private Animator animator;
    private bool PassiveListActivated = false;
    private PassiveSlot[] slots;
    [SerializeField]
    private GameObject go_SloatsParent;

    // Start is called before the first frame update
    void Start()
    {
        PassiveListActivated = false;
        animator = transform.GetComponent<Animator>();
        slots = go_SloatsParent.GetComponentsInChildren<PassiveSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenPassiveList();
    }

    private void TryOpenPassiveList()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PassiveListActivated = !PassiveListActivated;

            if (PassiveListActivated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        animator.SetTrigger("Open");
    }

    private void CloseInventory()
    {
        animator.SetTrigger("Close");
    }
    public void AcquireItem(PassiveItem _item, int _count = 1)
    {
        if (true)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].passiveItem != null)
                {
                    if (slots[i].passiveItem.PassiveItemName == _item.PassiveItemName)
                    {
                        slots[i].SetSlotCount(_count);
                        
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].passiveItem == null)
            {
                slots[i].AddItem(_item, _count);
                
                return;
            }
        }
    }

    public int CountItem(Item _item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].passiveItem == _item)
            {
                return slots[i].itemCount;
            }
        }
        return 0;
    }
}
