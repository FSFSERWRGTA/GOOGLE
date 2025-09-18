using UnityEngine;

public class ObjData : MonoBehaviour
{
    [Header("아이템 기본 정보")]
    public int id;                     // 아이템 ID (필요 없으면 제거 가능)
    public bool isNpc;                 // NPC 여부
    public bool canGet = true;         // ✅ 습득 가능 여부 (Inspector 체크박스)
    public string itemName;            // 아이템 이름
    [TextArea] public string itemDescription; // 아이템 설명
    public Sprite itemIcon;            // 아이템 아이콘
    public string npcName;             // NPC 이름 (NPC 오브젝트일 때)

    [Header("아이템 타입")]
    public ItemType itemType;

    public enum ItemType
    {
        Use,
        Quest,
    }
}