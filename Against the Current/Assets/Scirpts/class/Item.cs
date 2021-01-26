using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

}
