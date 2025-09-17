using UnityEditor;
using UnityEngine;

public class ObjData : MonoBehaviour
{
    public int id;        // TalkManager에서 쓰는 id
    public bool isNpc;
    public string itemName;
    public string itemDescription; //Item 성명
    public Sprite itemIcon;
    public string npcName; // ✅ 추가 (Inspector에서 NPC 이름 직접 입력)
    public ItemType itemType;

    public enum ItemType{
        Use,
        Equip,
        Quest,
        ETC
    }
    
    public ObjData(int _itemID, string _itemName, string _itemDescription, ItemType _itemType)
    {
        id = _itemID;
        itemName = _itemName;
        itemDescription = _itemDescription;
        itemIcon = Resources.Load("Sprites/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
        itemType = _itemType;
        
    }
}