using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSlot : MonoBehaviour
{
    public PassiveItem passiveItem;
    public Image image;
    public int itemCount;
    [SerializeField]
    private Text text_count;
    [SerializeField]
    private GameObject go_countImage;

    private void SetColor(float _alpha)
    {
        Color color = image.color;
        color.a = _alpha;
        image.color = color;
    }

    public void AddItem(PassiveItem _item, int _count = 1)
    {
        passiveItem = _item;
        itemCount = _count;
        image.sprite = passiveItem.itemImage;

        go_countImage.SetActive(true);
        text_count.text = itemCount.ToString();

        SetColor(1);
    }

    // 아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_count.text = itemCount.ToString();
    }
}
