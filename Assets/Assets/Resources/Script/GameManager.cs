using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public Text talkText;
    public GameObject talkPanel;
    public GameObject npcPanel;
    public Text npcText;
    public Text name;          
    public Image portraitImg;  

    [Header("Manager")]
    public TalkManager talkManager;
    public Inventory inventory;   // Inspector에서 Inventory 오브젝트 직접 연결
    
    public ChatManager chatManager;
    

    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc); 
    }

    void Talk(int id, bool isNpc)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);
        
        if (talkData == null)
        {
            // ✅ 대화가 끝났고, 아이템이면 인벤토리에 추가
            ObjData objData = scanObject.GetComponent<ObjData>();
            if (objData != null && objData.canGet)
            {
                if (inventory != null)
                {
                    inventory.AddItem(objData);
                    Debug.Log($"[아이템 습득 완료] {objData.itemName} 인벤토리에 추가됨!");
                }
                else
                {
                    Debug.LogError("[에러] Inventory 참조가 설정되지 않았습니다! GameManager Inspector 확인하세요.");
                }

                // 오브젝트 제거
                scanObject.SetActive(false);
            }

            isAction = false;
            talkIndex = 0;
            talkPanel.SetActive(false);
            npcPanel.SetActive(false);
            //chatManager.AddNarration(talkData);
            return;
        }

        if (isNpc)
        {
            talkPanel.SetActive(false);
            npcPanel.SetActive(true);

            string[] splitData = talkData.Split(':');

            string npcName = splitData[0];   // 이름
            string npcLine = splitData[1];   // 대사

            name.text = npcName;
            npcText.text = npcLine;

            Sprite npcPortrait = null;
            if (splitData.Length > 2)
            {
                int portraitIndex;
                if (int.TryParse(splitData[2], out portraitIndex))
                {
                    npcPortrait = talkManager.GetPortrait(id, portraitIndex);
                    if (portraitImg != null)
                    {
                        portraitImg.sprite = npcPortrait;
                        portraitImg.color = new Color(1, 1, 1, 1);
                    }
                }
            }

            // ✅ 채팅창에도 추가
            chatManager.AddChat(npcName, npcPortrait, npcLine);
        }

        else
        {
            talkPanel.SetActive(true);
            npcPanel.SetActive(false);

            talkText.text = talkData;
            chatManager.AddNarration(talkData);
        }
    
        isAction = true;
        talkIndex++;
    }
}
