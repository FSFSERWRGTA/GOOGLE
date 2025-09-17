using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;
    
    public Sprite[] portraitArr;
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();

    }

    // Update is called once per frame
    void GenerateData()
    {
        talkData.Add(1000, new string[]{"행인1:그거 들었어? 저기에 새로운 귀신이 돌아다닌다는 소문이 돌던데.:0", "행인2:뭐 정말?:1"});
        talkData.Add(100, new string[]{"평범한 문. 현재는 닫혀서 열리지 않는다."});
        
        portraitData.Add(1000 +0, portraitArr[0]);
        portraitData.Add(1000 +1, portraitArr[1]);
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }
}
