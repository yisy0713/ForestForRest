using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    private Animator animator;

    // ÇÊ¿äÇÑ ÄÄÆ÷³ÍÆ®
    [SerializeField]
    private GameObject go_inventoryBase;
    [SerializeField]
    private GameObject go_SloatsParent;

    // ½½·Ôµé
    private SlotUI[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventoryActivated = false;
        animator = GetComponent<Animator>();
        slots = go_SloatsParent.GetComponentsInChildren<SlotUI>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
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
        animator.SetTrigger("open");
    }

    private void CloseInventory()
    {
        animator.SetTrigger("close");
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if(Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        slots[i].AddTotalItemCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                slots[i].AddTotalItemCount(_count);
                return;
            }
        }
    }

    public int CountItem(Item _item)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == _item)
            {
                return slots[i].itemCount;
            }
        }
        return 0;
    }

    public int CountTotalItem(Item _item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == _item)
            {
                return slots[i].itemTotalCount;
            }
        }
        return 0;
    }

    public void DecreaseItemCount(Item _item, int _count)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == _item)
            {
                slots[i].SetSlotCount(_count);
            }
        }
    }

}
