using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public int value;

    public enum ItemType { Heal, Energy, Other }

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        // สำหรับ PC
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 worldPoint = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
                PickupItem();
        }

        // สำหรับมือถือ
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector3 worldPoint = mainCam.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
            Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
                PickupItem();
        }
    }

    void PickupItem()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("❌ ไม่มี GameManager ใน Scene!");
            return;
        }

        ItemData newItem = new ItemData(itemName, icon, type, value);
        GameManager.instance.AddItem(newItem);
        Debug.Log($"✅ เก็บไอเท็ม: {itemName}");
        Destroy(gameObject);
    }
}
