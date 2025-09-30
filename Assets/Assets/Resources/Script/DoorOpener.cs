using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    //문의 여닫힘 애니메이션을 처리하는 코드
    //플레이어의 소지 아이템 상태에 따라 잠김 상태 구현도 가능
    //컴포넌트
    AnimationController animCon;
    BoxCollider2D boxCol;
    [SerializeField] private Inventory inventory;

    //접근 변수
    [SerializeField] private bool isLock;           //잠겨있는지 여부. 참==잠김, 거짓==열림
    [SerializeField] private int itemCode;          //만약 잠겨있다면 문을 여는데 필요한 아이템 코드
    [SerializeField] private float rayDistance = 4f;             //레이 감지 거리
    [SerializeField] private float rayRadius = 0.5f;             //레이(구형) 반경
    [SerializeField] Transform rayOffset;                       //레이의 출발 위치치
    [ContextMenuItem("Left", "SetRayDirectionLeft")]
    [ContextMenuItem("Up", "SetRayDirectionUp")]
    [ContextMenuItem("Right", "SetRayDirectionRight")]
    [ContextMenuItem("Down", "SetRayDirectionDown")]
    [Tooltip("우클릭으로 방향 설정")]
    [SerializeField] private Vector2 directionSource;          //레이 방향 기준(없으면 이 오브젝트 forward)
    [SerializeField] private LayerMask raycastMask;             //인식할 레이어

    //비접근 변수
    private bool playerInSight = false;                          //레이 시야 내 플레이어 유무


    //함수수
    void Awake()
    {
        animCon = GetComponent<AnimationController>();
        TryGetComponent<BoxCollider2D>(out BoxCollider2D box);
        if (box != null) boxCol = box;
    }

    void Update()
    {
        //완전 개방 문이거나        특정 아이템을 소지한 상태인 경우 열림처리 활성화
        //if((isLock==false) || (isLock==true && ))
        if (isLock == false)
            PerformRaycastDetection();
    }

    //구형 레이캐스트 방식을 이용해 오브젝트 주변에 있는 물체가 플레이어인지 검사
    void PerformRaycastDetection()
    {
        Vector2 origin = (rayOffset != null) ? (Vector2)rayOffset.position : (Vector2)transform.position;
        Vector2 direction = (directionSource != null) ? directionSource : (Vector2)transform.up;

        // 2D 디버그 레이 표시(씬 뷰): Z는 0으로 고정
        Debug.DrawRay((Vector3)origin, new Vector3(direction.x, direction.y, -1f) * rayDistance, Color.green);

        bool sawPlayer = false;
        // 원형 단면 레이캐스트(2D): CircleCast는 RaycastHit2D를 반환
        RaycastHit2D hit = Physics2D.CircleCast(origin, rayRadius, direction, rayDistance, raycastMask);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                sawPlayer = true;
            }
        }

        if (sawPlayer && !playerInSight)
        {
            playerInSight = true;
            Debug.Log("레이(CircleCast)로 플레이어 접근을 감지했습니다.");
            animCon.SetAnimState(1);        //문 열림 애니메이션
            if (boxCol != null) boxCol.enabled = false;
        }
        else if (!sawPlayer && playerInSight)
        {
            playerInSight = false;
            Debug.Log("레이 시야(CircleCast)에서 플레이어가 사라졌습니다.");
            animCon.SetAnimState(0);        //문 닫힘 애니메이션
            if (boxCol != null) boxCol.enabled = true;
        }
    }

    //레이 방향 설정용
    void SetRayDirectionRight() {
        directionSource = (Vector2)transform.right;
    }
    void SetRayDirectionUp() {
        directionSource = (Vector2)transform.up;
    }
    void SetRayDirectionLeft() {
        directionSource = -1*(Vector2)transform.right;
    }
    void SetRayDirectionDown() {
        directionSource = -1*(Vector2)transform.up;
    }
}
