using UnityEngine;
using UnityEngine.SceneManagement;
 
public class Door_SceneMover : MonoBehaviour
{
    //씬 이동을 처리하는 코드드
    //플레이어의 소지 아이템 상태에 따라 잠김 상태 구현
    //컴포넌트
    [SerializeField] private Inventory inventory;

    //접근 변수
    [SerializeField] private int itemCode;          //문을 여는데 필요한 아이템 코드
    [SerializeField] private string sceneTarget;    //이동하고자 하는 목적지 씬 이름

    //비접근 변수
    bool isMovable = false;         //이동 가능한 상태인지 확인하기 위한 변수

    //함수
    void Update()
    {
        //완전 개방 문이거나        특정 아이템을 소지한 상태인 경우 열림처리 활성화
        if (isMovable == true && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneTarget);
        }
    }


    //콜라이더 접근 상태에 따라 이동 가능상태 설정
    void OnTriggerEnter2D(Collider2D obj) {
        if (obj.gameObject.CompareTag("Player") == true && inventory.SearchItem(itemCode) == true) isMovable = true;
        else isMovable = false;
    }
    
    void OnTriggerExit2D(Collider2D obj) {
        isMovable = false;
    }
}
