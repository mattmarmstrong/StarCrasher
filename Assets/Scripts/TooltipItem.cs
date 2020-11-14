using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipItem : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform;
    public GameObject display;
    private RectTransform rectTransform;
    public InventoryObject invObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        gameObject.SetActive(false);
        rectTransform = transform.GetComponent<RectTransform>();
        transform.SetParent(canvasRectTransform);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowTooltip(GameObject hoveredObject, InventorySlot inventorySlot)
    {
        if (inventorySlot.item.ID >= 0)
        {
            gameObject.SetActive(true);

            Vector2 tooltip_pos = hoveredObject.GetComponent<RectTransform>().position;
            int padding = 2;

            tooltip_pos.y += ((rectTransform.rect.height / 2) + (hoveredObject.GetComponent<RectTransform>().rect.height / 2) + padding) * canvasRectTransform.localScale.y;
            tooltip_pos.x += ((rectTransform.rect.width / 2) - (hoveredObject.GetComponent<RectTransform>().rect.width / 2)) * canvasRectTransform.localScale.x;

            if (tooltip_pos.x + ((rectTransform.rect.width/2) * canvasRectTransform.localScale.x) > // Right edge of tooltip
                (canvasRectTransform.rect.width * canvasRectTransform.localScale.x)) // Right edge of canvas/screen
            {
                tooltip_pos.x -= tooltip_pos.x + ((rectTransform.rect.width / 2) * canvasRectTransform.localScale.x) - 
                    (canvasRectTransform.rect.width * canvasRectTransform.localScale.x);
            }
            if (tooltip_pos.y + ((rectTransform.rect.height / 2) * canvasRectTransform.localScale.y) > // Top edge of tooltip
                (canvasRectTransform.rect.height * canvasRectTransform.localScale.y)) // Top edge of canvas/screen
            {
                tooltip_pos.y -= tooltip_pos.y + ((rectTransform.rect.height / 2) * canvasRectTransform.localScale.y) - 
                    (canvasRectTransform.rect.height * canvasRectTransform.localScale.y);
            }

            rectTransform.position = tooltip_pos;


            var attributeText = transform.Find("Attributes").GetComponent<TextMeshProUGUI>();
            var attributeValueText = transform.Find("Attributes").GetChild(0).GetComponent<TextMeshProUGUI>();

            InventorySlot slot = inventorySlot;

            foreach (var item in invObject.Container.Items)
            {
                if (item == inventorySlot)
                {
                    slot = item;
                }
            }

            foreach (var buff in slot.ItemObject.data.buffs)
            {
                switch (buff.attribute)
                {
                    case Attributes.Stamina:
                        attributeText.text += "Stamina\n";
                        break;
                    case Attributes.Strength:
                        attributeText.text += "Strength\n";
                        break;
                    case Attributes.Agility:
                        attributeText.text += "Agility\n";
                        break;
                    case Attributes.Intellect:
                        attributeText.text += "Intellect\n";
                        break;
                    default:
                        Debug.Log("defaulted switch statement in ShowTooltip()", this);
                        break;
                }
                attributeValueText.text += buff.value.ToString() + "\n";
            }

            transform.Find("nameText").GetComponent<TextMeshProUGUI>().text = slot.ItemObject.name;
            transform.Find("Image").GetComponent<Image>().sprite = slot.ItemObject.uiDisplay;
            transform.Find("descriptionText").GetComponent<TextMeshProUGUI>().text = slot.ItemObject.description;
        }
    }

    public void HideTooltip()
    {
        transform.Find("Attributes").GetComponent<TextMeshProUGUI>().text = "";
        transform.Find("Attributes").GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        gameObject.SetActive(false);
    }

}
