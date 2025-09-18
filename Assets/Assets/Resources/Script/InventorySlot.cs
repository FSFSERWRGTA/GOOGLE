using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text itemName_Text;
    public GameObject selected_Item;

    private ObjData currentItem;
    private int slotIndex;
    private Inventory inventory;

    // 슬롯 초기화 (버튼 연결)
    public void Init(int index, Inventory inven)
    {
        slotIndex = index;
        inventory = inven;

        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                if (currentItem != null)
                {
                    inventory.OnClickSlot(currentItem);
                }
            });
        }

        Clear();
    }

    public void AddItem(ObjData _item)
    {
        currentItem = _item;
        itemName_Text.text = _item.itemName;
        icon.sprite = _item.itemIcon;
        icon.enabled = true;
    }

    public void Clear()
    {
        currentItem = null;
        itemName_Text.text = "";
        icon.sprite = null;
        icon.enabled = false;
    }
}