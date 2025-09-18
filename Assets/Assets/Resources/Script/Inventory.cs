using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject go;                      // 인벤토리 전체 패널
    public Image descriptionImg;               // 설명 아이콘
    public Text descriptionName;               // 설명 이름 
    public Text descriptionText;               // 설명 텍스트
    public GameObject[] tabButtons;            // 탭 버튼들 (Inspector에서 할당)
    public InventorySlot[] slots;              // 슬롯들 (Grid 밑에 배치 후 드래그)

    [Header("탭 설명")]
    public string[] tabDescription;            // 탭별 설명 텍스트

    // ✅ 리스트 초기화
    private List<ObjData> inventoryItemList = new List<ObjData>();   // 전체 아이템
    private List<ObjData> inventoryTabList = new List<ObjData>();    // 현재 탭에 표시되는 아이템

    private int selectedTab;
    private bool activated;

    private void Awake()
    {
        // go를 Inspector에서 안 넣으면 자동으로 자기 자신을 참조
        if (go == null)
            go = this.gameObject;
    }

    private void Start()
    {
        // 슬롯 초기화
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Init(i, this);
        }

        // 처음엔 닫혀있음
        go.SetActive(false);
        ClearDescription(); // 시작 시 설명 초기화
    }

    // ✅ 아이템 추가
    public void AddItem(ObjData item)
    {
        if (item == null)
        {
            Debug.LogWarning("[인벤토리] null 아이템 추가 시도됨");
            return;
        }

        inventoryItemList.Add(item);
        Debug.Log($"[인벤토리] {item.itemName} 추가됨. 현재 개수: {inventoryItemList.Count}");

        ShowItem();
    }

    // ✅ 인벤토리 열기
    public void OpenInventory()
    {
        activated = true;
        go.SetActive(true);

        selectedTab = 0; // 항상 0번째 탭부터
        ShowTab();
    }

    // ✅ 인벤토리 닫기
    public void CloseInventory()
    {
        activated = false;
        go.SetActive(false);

        ClearDescription(); // 닫을 때 설명 초기화
    }

    // ✅ 탭 클릭 시 (UI 버튼 OnClick에서 index 넘겨줌)
    public void OnClickTab(int tabIndex)
    {
        selectedTab = tabIndex;
        ShowTab();
    }

    // ✅ 슬롯 클릭 시 (아이템 상세 표시)
    public void OnClickSlot(ObjData item)
    {
        if (descriptionImg != null)
            descriptionImg.sprite = item.itemIcon;

        if (descriptionName != null)
            descriptionName.text = item.itemName;

        if (descriptionText != null)
            descriptionText.text = item.itemDescription;

        Debug.Log($"[인벤토리] {item.itemName} 클릭됨 -> 상세정보 표시 완료");
    }

    // ✅ 탭 표시
    private void ShowTab()
    {
        // 탭 버튼 알파값 조정
        for (int i = 0; i < tabButtons.Length; i++)
        {
            var img = tabButtons[i].GetComponent<Image>();
            if (img != null)
            {
                Color c = img.color;
                c.a = (i == selectedTab) ? 0.5f : 1f;
                img.color = c;
            }
        }

        // 탭 전환 시 설명 지우기
        ClearDescription();

        // 탭 설명 로그 출력 (필요시 UI로도 표시 가능)
        if (tabDescription != null && selectedTab < tabDescription.Length)
        {
            Debug.Log($"[인벤토리] {tabDescription[selectedTab]} 탭 선택됨");
        }

        ShowItem();
    }

    // ✅ 슬롯에 아이템 보여주기
    private void ShowItem()
    {
        // 리스트 초기화
        inventoryTabList.Clear();
        foreach (var slot in slots) slot.Clear();

        // 현재 탭에 맞는 아이템만 추가
        foreach (var item in inventoryItemList)
        {
            if (selectedTab == 0 && item.itemType == ObjData.ItemType.Use)
                inventoryTabList.Add(item);
            else if (selectedTab == 1 && item.itemType == ObjData.ItemType.Quest)
                inventoryTabList.Add(item);
        }

        // 슬롯에 표시
        for (int i = 0; i < inventoryTabList.Count && i < slots.Length; i++)
        {
            slots[i].AddItem(inventoryTabList[i]);
        }
    }

    // ✅ 설명 초기화 함수
    private void ClearDescription()
    {
        if (descriptionImg != null)
            descriptionImg.sprite = null;

        if (descriptionName != null)
            descriptionName.text = "";

        if (descriptionText != null)
            descriptionText.text = "";
    }
}
