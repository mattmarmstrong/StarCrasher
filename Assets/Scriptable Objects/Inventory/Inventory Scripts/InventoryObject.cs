using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{

    public ItemDatabaseObject database;
    public Inventory Container;
    public bool AddItem(Item item, int amount)
    {
        //if (EmptySlotCount <= 0)
        //    return false;   // Inventory full
        
        InventorySlot slot = FindInventorySlot(item);
        if (!database.Items[item.ID].stackable || slot == null)
        {
            if (EmptySlotCount <= 0)
                return false;   // Inventory full
            SetToEmptySlot(item, amount);
            return true;
        }
        slot.AddAmount(amount);
        return true;
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].item.ID <= -1)
                    counter++;
            }
            return counter;
        }
    }

    // Finds InventorySlot that passed Item is in, else null
    public InventorySlot FindInventorySlot(Item item)
    {
        for (int i = 0; i < Container.Items.Length; i ++)
        {
            if (Container.Items[i].item.ID == item.ID)
                return Container.Items[i];
                
        }
        return null;
    }

    public InventorySlot SetToEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.ID <= -1)
            {
                Container.Items[i].UpdateSlot(item, amount);
                return Container.Items[i];
            }
        }
        //set up function for full inventory
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            ModifyOwnerStats(item1, item2);
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }

    }

    public void ModifyOwnerStats(InventorySlot item1, InventorySlot item2)
    {
        if (!(item1.parent is StaticInterface && item2.parent is StaticInterface))
        {
            if (item1.parent is StaticInterface)
            {
                if (!(item1.ItemObject == null || item1.ItemObject.data.ID < 0))
                {
                    foreach (var buff in item1.ItemObject.data.buffs)
                    {
                        RemoveStatsOwner(item1.parent.ownership.GetComponent<Player>(), buff);
                    }
                }
                if (!(item2.ItemObject == null || item2.ItemObject.data.ID < 0))
                {
                    foreach (var buff in item2.ItemObject.data.buffs)
                    {
                        AddStatsOwner(item1.parent.ownership.GetComponent<Player>(), buff);
                    }
                }
                item1.parent.transform.Find("Stamina").GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                    item1.parent.ownership.GetComponent<Player>().Stamina.ToString();

                item1.parent.transform.Find("Strength").GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                    item1.parent.ownership.GetComponent<Player>().Strength.ToString();

                item1.parent.transform.Find("Intellect").GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                    item1.parent.ownership.GetComponent<Player>().Intellect.ToString();

                item1.parent.transform.Find("Agility").GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                    item1.parent.ownership.GetComponent<Player>().Agility.ToString();

            }
            else if (item2.parent is StaticInterface)
            {
                if (!(item2.ItemObject == null || item2.ItemObject.data.ID < 0))
                {
                    foreach (var buff in item2.ItemObject.data.buffs)
                    {
                        RemoveStatsOwner(item2.parent.ownership.GetComponent<Player>(), buff);
                    }
                }
                if (!(item1.ItemObject == null || item1.ItemObject.data.ID < 0))
                {
                    foreach (var buff in item1.ItemObject.data.buffs)
                    {
                        //Debug.Log(item2.parent.ownership.GetComponent<Player>().Stamina.ToString());
                        //Debug.Log(buff.attribute + " " + buff.value);
                        AddStatsOwner(item2.parent.ownership.GetComponent<Player>(), buff);
                    }
                }
                item2.parent.transform.Find("Stamina").GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                    item2.parent.ownership.GetComponent<Player>().Stamina.ToString();

                item2.parent.transform.Find("Strength").GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                    item2.parent.ownership.GetComponent<Player>().Strength.ToString();

                item2.parent.transform.Find("Intellect").GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                    item2.parent.ownership.GetComponent<Player>().Intellect.ToString();

                item2.parent.transform.Find("Agility").GetChild(0).GetComponent<TextMeshProUGUI>().text = 
                    item2.parent.ownership.GetComponent<Player>().Agility.ToString();
            }
        }
    }

    public void AddStatsOwner(Player owner, ItemBuff buff)
    {
        switch (buff.attribute)
        {
            case Attributes.Agility:
                owner.Agility += buff.value;
                break;
            case Attributes.Intellect:
                owner.Intellect += buff.value;
                break;
            case Attributes.Stamina:
                owner.Stamina += buff.value;
                break;
            case Attributes.Strength:
                owner.Strength += buff.value;
                break;
            default:
                Debug.Log("defaulted switch statement in AddStatsOwner()", this);
                break;
        }
    }

    public void RemoveStatsOwner(Player owner, ItemBuff buff)
    {
        switch (buff.attribute)
        {
            case Attributes.Agility:
                owner.Agility -= buff.value;
                break;
            case Attributes.Intellect:
                owner.Intellect -= buff.value;
                break;
            case Attributes.Stamina:
                owner.Stamina -= buff.value;
                break;
            case Attributes.Strength:
                owner.Strength -= buff.value;
                break;
            default:
                Debug.Log("defaulted switch statement in RemoveStatsOwner()", this);
                break;
        }
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item == item)
            {
                Container.Items[i].UpdateSlot(null, 0);
            }
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }

    
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[24];
    public void Clear() 
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].RemoveItem();
        }
    }
}

[System.Serializable] 
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];

    [System.NonSerialized]
    public UserInterface parent;
    public Item item;
    public int amount;

    public InventorySlot()
    {
        this.item = new Item();
        this.amount = 0;
    }

    public InventorySlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public ItemObject ItemObject
    {
        get
        {
            if (item.ID >= 0)
            {
                return parent.inventory.database.Items[item.ID];
            }
            return null;
        }
    }

    public void UpdateSlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public void RemoveItem()
    {
        item = new Item();
        amount = 0;
    }

    public void AddAmount(int value)
    {
        this.amount += value;
    }

    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        if (AllowedItems.Length <= 0 || itemObject == null || itemObject.data.ID < 0)
            return true;
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (itemObject.type == AllowedItems[i])
                return true;
        }
        return false;
    }
}
/* -Static class for mousedata
 * -itemobject no longer has id, only item contains id
 * -additem change
 * 
 */