using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public GameManager manager;          // MoveCharacter와 연결된 GameManager

    private InventorySlot[] slots;
    
    private List<ObjData> inventoryItemList; 
    private List<ObjData> inventoryTabList;  

    public Text Description_Text;        
    public string[] tabDescription;      

    public Transform tf;                 
    public GameObject go;                
    public GameObject[] selectedTabImages;

    private int selectedItem;
    private int selectedTab;

    private bool activated;    
    private bool tabActivated; 
    private bool itemActivated;
    private bool stopKeyInput; 
    private bool preventExec;  

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    void Start()
    {
        inventoryItemList = new List<ObjData>();
        inventoryTabList = new List<ObjData>();
        slots = tf.GetComponentsInChildren<InventorySlot>();
    }
    
    public void ShowTab() //탭 활성화
    {
        RemoveSlot();
        SelectedTab();
    }

    public void RemoveSlot() //인벤토리 슬롯 초기화
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Clear();
            slots[i].gameObject.SetActive(false);
        }
    }

    public void SelectedTab() //선택된 탭을 제외하고 다른 모든 탭의 컬러 알파값 0으로 조정
    {
        StopAllCoroutines();

        for (int i = 0; i < selectedTabImages.Length; i++)
        {
            Color baseColor = selectedTabImages[i].GetComponent<Image>().color;
            baseColor.a = (i == selectedTab) ? 0f : 1f;
            selectedTabImages[i].GetComponent<Image>().color = baseColor;
        }

        Description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    }

    IEnumerator SelectedTabEffectCoroutine() //선택 탭 반짝임 효과
    {
        while (tabActivated)
        { 
            Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;

            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }

            while (color.a > 0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void ShowItem() //아이템 활성화(inventorylist에 조건에 맞는 아이템을 넣어주고, 인벤토리 슬롯에 풀력)
    {
        inventoryTabList.Clear();
        RemoveSlot();
        selectedItem = 0;

        switch (selectedTab)
        {
            case 0:
                for (int i = 0; i < inventoryItemList.Count; i++)
                    if (inventoryItemList[i].itemType == ObjData.ItemType.Use)
                        inventoryTabList.Add(inventoryItemList[i]);
                break;

            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                    if (inventoryItemList[i].itemType == ObjData.ItemType.Equip)
                        inventoryTabList.Add(inventoryItemList[i]);
                break;

            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                    if (inventoryItemList[i].itemType == ObjData.ItemType.Quest)
                        inventoryTabList.Add(inventoryItemList[i]);
                break;

            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                    if (inventoryItemList[i].itemType == ObjData.ItemType.ETC)
                        inventoryTabList.Add(inventoryItemList[i]);
                break;
        }

        for (int i = 0; i < inventoryTabList.Count; i++) 
        {
            slots[i].gameObject.SetActive(true);
            slots[i].AddItem(inventoryTabList[i]);
        }

        SelectedItem();
    }
    
    public void SelectedItem() //선택된 아이템을 제외하고 다른 모든 아이템의 컬러 알파값을 0으로 조정
    {
        StopAllCoroutines();
        if (inventoryTabList.Count > 0)
        {
            Color color = slots[selectedItem].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i < inventoryTabList.Count; i++)
            {
                slots[i].selected_Item.GetComponent<Image>().color = color;
            }
            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemCoroutine());
        }
        else
        {
            Description_Text.text = "해당 타입의 아이템을 소유하고있지 않습니다.";
        }
    }

    IEnumerator SelectedItemCoroutine() //선택된 아이템 반짝임 효과
    {
        while (itemActivated)
        {
            Color color = slots[selectedItem].selected_Item.GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }

            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            
            yield return new WaitForSeconds(0.3f);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activated = !activated;
            if (activated)
            {
                manager.isAction = true;   // 플레이어 이동 막기
                go.SetActive(true);
                selectedTab = 0;
                tabActivated = true;
                itemActivated = true;
                ShowTab();
            }
            else
            {
                StopAllCoroutines();
                go.SetActive(false);
                tabActivated = false;
                itemActivated = false;
                manager.isAction = false;  // 플레이어 이동 허용
            }
        }

        if (activated)
        {
            if (tabActivated)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (selectedTab < selectedTabImages.Length - 1)
                        selectedTab++;
                    else
                        selectedTab = 0;

                    SelectedTab();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectedTab < selectedTabImages.Length - 1)
                        selectedTab++;
                    else
                        selectedTab = 0;

                    SelectedTab();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                    color.a = 0.25f;
                    selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                    itemActivated = true;
                    tabActivated = false;
                    preventExec = true;
                    ShowItem();
                }
            }
            
            else if(itemActivated)
            {
                 if(Input.GetKeyDown(KeyCode.Z))
            }
        }
    }
}
