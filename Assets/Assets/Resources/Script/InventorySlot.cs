using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image icon;
    public Text itmeName_Text;
    public Text itmeCost_Text;
    public GameObject selected_Item;

    public void AddItem(ObjData _item)
    {
        itmeName_Text.text = _item.itemName;
        icon.sprite = _item.itemIcon;
    }

    public void RemoveItem(ObjData _item)
    {
        itmeName_Text.text = "";
        icon.sprite = null;
    }
    public void Clear()
    {
        itmeName_Text.text = "";
        icon.sprite = null;
    }

}
