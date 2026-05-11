using UnityEngine;
using UnityEngine.UI;

public class InventoryHotbarUI : MonoBehaviour
{
    [SerializeField] private GameObject[] serializedSlots = new GameObject[0];
    [SerializeField] private Transform hotbarRoot;

    private GameObject[] slots;
    private Button[] slotButtons;
    private Image[] slotIcons;
    private bool subscribed;
    private bool initialized;

    public void Initialize(GameObject[] hotbarSlots)
    {
        slots = ResolveSlots(hotbarSlots);
        initialized = true;
        CacheSlots();
        Subscribe();

        if (GameManager.instance != null)
        {
            GameManager.instance.slots = slots;
            GameManager.instance.RegisterHotbarUI(this);
        }

        Refresh();
    }

    private void Start()
    {
        if (!initialized)
        {
            Initialize(serializedSlots);
        }
    }

    public void Refresh()
    {
        if (slotIcons == null || Services.Inventory == null)
        {
            return;
        }

        for (int i = 0; i < slotIcons.Length; i++)
        {
            if (slotIcons[i] == null)
            {
                continue;
            }

            if (i < Services.Inventory.Items.Count)
            {
                slotIcons[i].sprite = Services.Inventory.Items[i].Icon;
                slotIcons[i].color = Color.white;
            }
            else
            {
                slotIcons[i].sprite = null;
                slotIcons[i].color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        if (!subscribed || Services.Inventory == null)
        {
            return;
        }

        Services.Inventory.OnInventoryChanged -= Refresh;
        subscribed = false;
    }

    private void Subscribe()
    {
        if (subscribed || Services.Inventory == null)
        {
            return;
        }

        Services.Inventory.OnInventoryChanged += Refresh;
        subscribed = true;
    }

    private void CacheSlots()
    {
        slotButtons = new Button[slots.Length];
        slotIcons = new Image[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                continue;
            }

            slotButtons[i] = slots[i].GetComponent<Button>();
            Transform iconTransform = slots[i].transform.Find("Icon");
            if (iconTransform != null)
            {
                slotIcons[i] = iconTransform.GetComponent<Image>();
            }

            int index = i;
            if (slotButtons[i] != null)
            {
                slotButtons[i].onClick.RemoveAllListeners();
                slotButtons[i].onClick.AddListener(() => Services.Inventory?.UseItemAt(index));
            }
        }
    }

    private GameObject[] ResolveSlots(GameObject[] configuredSlots)
    {
        if (configuredSlots != null && configuredSlots.Length > 0)
        {
            return configuredSlots;
        }

        Transform resolvedRoot = hotbarRoot != null ? hotbarRoot : transform.Find("Hotbar");
        if (resolvedRoot == null)
        {
            return new GameObject[0];
        }

        GameObject[] discoveredSlots = new GameObject[resolvedRoot.childCount];
        for (int i = 0; i < resolvedRoot.childCount; i++)
        {
            discoveredSlots[i] = resolvedRoot.GetChild(i).gameObject;
        }

        return discoveredSlots;
    }
}
