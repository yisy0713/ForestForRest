using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite itemImage;
    public GameObject[] itemPrefabs;

    public string weaponType;

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        trash,
        ETC
    }
}
