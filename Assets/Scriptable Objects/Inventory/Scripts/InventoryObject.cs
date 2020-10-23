using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{

    public ItemDatabaseObject database;
    public Inventory Container;
    public bool AddItem(Item item, int amount)
    {
        if (EmptySlotCount <= 0)
            return false;   // Inventory full
        
        InventorySlot slot = FindInventorySlot(item);
        if (!database.Items[item.ID].stackable || slot == null)
        {
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
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
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