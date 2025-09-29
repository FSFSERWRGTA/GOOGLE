using UnityEngine;

public class EvidencePicker : MonoBehaviour
{
    //증거 이미지의 호출을 담당하는 스크립트트
    private ObjData objData;
    //private Inventory inventory;
    [SerializeField] private EvidenceShower evidenceShower;

    void Start()
    {
        objData = GetComponent<ObjData>();
        //inventory = FindObjectOfType<Inventory>();
        //evidenceShower = FindObjectOfType<EvidenceShower>();
    }
    
    //임의로 증거 이미지 출력을 위한 수정 처리
    public void TryImgShow()
    {
        if (objData != null && objData.canGet && evidenceShower != null)
        {
            evidenceShower.ShowEvidenceImg(objData.itemIcon);
            Debug.Log($"[이미지 출력 완료] {objData.itemName}");
        }
        else
        {
            Debug.LogWarning("[습득 실패] objData, evidenceShower 중 하나가 null/false");
        }
    }
/*
            private void OnTriggerStay2D(Collider2D other)
            {
                if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space))
                {
                    TryPickup();
                }
            }

            private void TryPickup()
            {
                Debug.Log("[디버그] TryPickup 호출됨");

                if (objData != null && objData.canGet && inventory != null)
                {
                    Debug.Log($"[습득 시도] {objData.itemName} (ID:{objData.id})");
                    inventory.AddItem(objData);
                    Debug.Log($"[인벤토리 전달 완료] {objData.itemName}");
                    evidenceShower.ShowEvidenceImg(objData.itemIcon);
                    Debug.Log($"[이미지 출력 완료] {objData.itemName}");
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("[습득 실패] objData, canGet, inventory 중 하나가 null/false");
                }
            }
        */
}
