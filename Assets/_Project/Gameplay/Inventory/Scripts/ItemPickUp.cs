using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    [Header("Definition")]
    [SerializeField] private ItemDefinition definition;

    [Header("Item Info")]
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public int value;

    public enum ItemType
    {
        Heal,
        Energy,
        Baterry,
    }

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (mainCam == null)
        {
            return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryPickupAtScreenPoint(Mouse.current.position.ReadValue());
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            TryPickupAtScreenPoint(Touchscreen.current.primaryTouch.position.ReadValue());
        }
    }

    private void TryPickupAtScreenPoint(Vector2 screenPoint)
    {
        Vector3 worldPoint = mainCam.ScreenToWorldPoint(screenPoint);
        Vector2 touchPosition = new Vector2(worldPoint.x, worldPoint.y);
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            PickupItem();
        }
    }

    private void PickupItem()
    {
        if (Services.Inventory == null)
        {
            Debug.LogError("No InventoryManager service is available.");
            return;
        }

        if (!Services.Inventory.CanAddItem)
        {
            Debug.Log("Inventory is full.");
            return;
        }

        ItemData newItem = definition != null
            ? new ItemData(definition)
            : new ItemData(itemName, icon, type, value);

        if (Services.Inventory.AddItem(newItem))
        {
            Debug.Log($"Picked up item: {newItem.ItemName}");
            Destroy(gameObject);
        }
    }
}
