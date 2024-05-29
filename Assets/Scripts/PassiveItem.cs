using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Item", menuName = "New Item/passive item")]
public class PassiveItem : ScriptableObject
{
    public string PassiveItemName;
    //public int PassiveId;
    //public ItemType itemType;
    public Sprite itemImage;
    public GameObject itemPrefab;

    //public string weaponType;

    /*public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        trash,
        ETC
    }*/
}
