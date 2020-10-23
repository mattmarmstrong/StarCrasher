using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ItemType
{
    Food,
    Helmet,
    Weapon,
    Shield,
    Chest,
    Boots,
    Default
}

public enum Attributes
{
    Agility,
    Intellect,
    Stamina,
    Strength
}

[CreateAssetMenu(fileName ="New Item", menuName = "Inventory System/Items/item")]
public class ItemObject : ScriptableObject
{
    public Sprite uiDisplay;
    public bool stackable;
    
    public ItemType type;
    [TextArea(15,20)]
    public string description;
    public Item data = new Item();


    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }

}

[System.Serializable]
public class Item
{
    public string name;
    public int ID;
    public ItemBuff[] buffs;

    public Item()
    {
        this.name = "";
        this.ID = -1;
    }

    public Item(ItemObject itemObject)
    {
        name = itemObject.name;
        ID = itemObject.data.ID;
        buffs = new ItemBuff[itemObject.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(itemObject.data.buffs[i].min, itemObject.data.buffs[i].max);
            buffs[i].attribute = itemObject.data.buffs[i].attribute;
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public Attributes attribute;
    public int value;
    public int min;
    public int max;

    public ItemBuff(int min, int max)
    {
        this.min = min;
        this.max = max;
        GenerateValue();
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}
