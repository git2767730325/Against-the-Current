using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        equipment=0,
        comsumables,
        material
    }
    public ItemType itemType;
    public int itemId;
    public Sprite sprite;
    public int itemPrice=0;
    //public float duarable;
}
