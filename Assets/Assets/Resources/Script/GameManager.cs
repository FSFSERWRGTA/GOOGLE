using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text talkText;
    public GameObject talkPanel;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;
    public TalkManager talkManager;
    
    public GameObject npcPanel;
    public Text npcText;
    public Text name;          // NPC 이름 표시용
    public Image portraitImg;  // NPC 초상화

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc); 
    }

    void Talk(int id, bool isNpc)   // ✅ int로 수정
    {
        string talkData = talkManager.GetTalk(id, talkIndex);
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            talkPanel.SetActive(false);
            npcPanel.SetActive(false);
            return;
        }

        if (isNpc)
        {
            talkPanel.SetActive(false);
            npcPanel.SetActive(true);

            string[] splitData = talkData.Split(':');
            npcText.text = splitData[1];
            name.text = splitData[0];  // ✅ ObjData에서 가져오기

            if (splitData.Length > 1)
            {
                int portraitIndex = int.Parse(splitData[2]);
                portraitImg.sprite = talkManager.GetPortrait(id, portraitIndex);
                portraitImg.color = new Color(1, 1, 1, 1);
            }
        }
        else
        {
            talkPanel.SetActive(true);
            npcPanel.SetActive(false);

            talkText.text = talkData;
        }
    
        isAction = true;
        talkIndex++;
    }

}