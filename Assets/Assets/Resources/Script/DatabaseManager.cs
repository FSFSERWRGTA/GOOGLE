using UnityEngine;
using System.Collections;
using System.Collections.Generic;  // List<T> 사용을 위해 필요

public class DatabaseManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static DatabaseManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches; // swiches → switches로 수정 권장

    public List<ObjData> itemList = new List<ObjData>();

    void Start()
    {
        itemList.Add(new ObjData(10005,"컴퓨터", "어떠한 용도로 쓰는 컴퓨터", ObjData.ItemType.Quest));
    }
}