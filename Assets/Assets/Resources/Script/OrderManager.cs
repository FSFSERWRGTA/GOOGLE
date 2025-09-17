using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private MoveCharacter player; // 실제 캐릭터 조작 스크립트 연결

    void Start()
    {
        player = FindObjectOfType<MoveCharacter>(); 
        // ⚠️ PlayerController 대신 실제 플레이어 이동 스크립트 이름을 넣어야 합니다.
    }

    // 플레이어 이동 막기
    public void NoMove()
    {
        if (player != null)
            player.canMove = false; // PlayerController 안에 canMove 같은 bool 필요
    }

    // 플레이어 이동 허용
    public void Move()
    {
        if (player != null)
            player.canMove = true;
    }
}