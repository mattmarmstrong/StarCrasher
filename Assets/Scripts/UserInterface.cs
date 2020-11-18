using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    public ObjectWithStatsOrInventory ownership;
    public TooltipItem tooltip_Item;

    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            inventory.Container.Items[i].parent = this;
        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    // Update is called once per frame
    void Update()
    {
        slotsOnInterface.UpdateSlotDisplay();
    }

    public void UpdateDisplay()
    {

    }

    // Different funcitonality for DynamicInterface and StaticInterface
    public abstract void CreateSlots();

    // Adds interactability to inventory slots
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    // Mouse hovers over an inventory slot
    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
        if (slotsOnInterface[obj] is InventorySlot)
        {
            tooltip_Item.ShowTooltip(obj, slotsOnInterface[obj]);
        }
    }

    // Mouse hovers off of inventory slot
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
        tooltip_Item.HideTooltip();
    }

    // Mouse hovers over an interface
    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    // Mouse hovers off of interface
    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    // Creates temp item sprite that follows mouse
    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        
        if (slotsOnInterface[obj].item.ID >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 100);
            tempItem.transform.SetParent(transform.parent);
            
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;  //inventory.database.GetItem[slotsOnInterface[obj].item.ID].uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }

    // Attempts to swap item to inventory slot hovered, deletes if outside interface
    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);   // from, to
        }
    }

    // Temp item sprite follows mouse
    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

}

// For use in referencing mouse events
public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}


public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in slotsOnInterface)
        {
            if (slot.Value.item.ID >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.ItemObject.uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}