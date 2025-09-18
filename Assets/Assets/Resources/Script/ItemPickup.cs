using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private ObjData objData;
    private Inventory inventory;

    void Start()
    {
        objData = GetComponent<ObjData>();
        inventory = FindObjectOfType<Inventory>();
    }

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
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("[습득 실패] objData, canGet, inventory 중 하나가 null/false");
        }
    }


}