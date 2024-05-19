using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Store Item", menuName = "Store/Item")]
public class StoreItem : ScriptableObject
{
    public List<ItemInfo> items = new List<ItemInfo>();
}

[System.Serializable]
public class ItemInfo
{
    public string itemName;
    public string description;
    public int price;
    public Sprite itemImage;

    public ItemStatus itemStatus;

    public int Quantity;
}

public enum ItemStatus
{
    Available,
    SoldOut,
    OutOfStock,
}
