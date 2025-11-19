using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player State")]
    public Vector3 lastPlayerPosition;
    public bool isDead = false;
    public int health = 100;
    public int maxHealth = 100;
    public float energy = 100f;
    public float maxEnergy = 100f;
    public GameObject deathScreen;

    [Header("Door Lock System")]
    public HashSet<string> lockedDoors = new HashSet<string>();

    [Header("Damage Overlay")]
    public Image damageOverlay;
    public float overlayDuration = 0.5f;
    public float overlayMaxAlpha = 0.5f;
    private Coroutine overlayCoroutine;

    [Header("Energy Regeneration")]
    public bool energyRegenEnabled = true;
    public float walkRegenRate = 2f;
    public float idleRegenRate = 5f;
    public float energyRegenDelay = 1.5f;
    private float regenTimer = 0f;

    [Header("Flashlight System")]
    public bool flashlightOn = false;
    public float flashlightPower = 100f;
    public float maxFlashlightPower = 100f;
    public float flashlightDrainRate = 10f;
    public Light2D flashlight2D;
    public TMP_Text flashlightText;

    [Header("UI Sliders")]
    public Slider healthSlider;
    public Slider energySlider;

    [Header("Movement Flags")]
    public bool isMoving = false;
    public bool isRunning = false;

    [Header("Inventory System")]
    public List<ItemData> inventory = new List<ItemData>();
    public int maxInventorySize = 3;

    [Header("Hotbar UI (3 Slots)")]
    public GameObject[] slots = new GameObject[3];
    private Button[] slotButtons;
    private Image[] slotIcons;


    // ------------------------------------------------------------
    // Awake (Singleton)
    // ------------------------------------------------------------
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // ------------------------------------------------------------
    // Update Loop
    // ------------------------------------------------------------
    private void Update()
    {
        HandleEnergySystem();
        HandleFlashlight();
        UpdateUI();

        if (health <= 0 && !isDead)
        {
            energy = maxEnergy;
            health = maxHealth;

            inventory.Clear();
            RefreshHotbar();

            deathScreen?.SetActive(true);

            isDead = true;
        }
    }


    // ------------------------------------------------------------
    // HP & Damage System
    // ------------------------------------------------------------
    public void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);

        if (overlayCoroutine != null)
            StopCoroutine(overlayCoroutine);

        overlayCoroutine = StartCoroutine(FlashDamageOverlay());
    }

    private IEnumerator FlashDamageOverlay()
    {
        if (damageOverlay == null) yield break;

        Color color = damageOverlay.color;

        // Fade In
        float t = 0f;
        while (t < overlayDuration / 2)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, overlayMaxAlpha, t / (overlayDuration / 2));
            damageOverlay.color = color;
            yield return null;
        }

        // Fade Out
        t = 0f;
        while (t < overlayDuration / 2)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(overlayMaxAlpha, 0, t / (overlayDuration / 2));
            damageOverlay.color = color;
            yield return null;
        }

        color.a = 0;
        damageOverlay.color = color;
    }

    public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public void UseEnergy(float amount)
    {
        energy = Mathf.Clamp(energy - amount, 0, maxEnergy);
        regenTimer = 0f;
    }


    // ------------------------------------------------------------
    // Energy Regeneration
    // ------------------------------------------------------------
    private void HandleEnergySystem()
    {
        if (!energyRegenEnabled || maxEnergy <= 0f) return;
        if (isRunning)
        {
            regenTimer = 0f;
            return;
        }

        if (energy < maxEnergy)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= energyRegenDelay)
            {
                float regenRate = isMoving ? walkRegenRate : idleRegenRate;
                energy = Mathf.Clamp(energy + regenRate * Time.deltaTime, 0, maxEnergy);
            }
        }
    }


    // ------------------------------------------------------------
    // Flashlight System
    // ------------------------------------------------------------
    private void HandleFlashlight()
    {
        if (flashlightOn && flashlightPower > 0f)
        {
            flashlightPower -= flashlightDrainRate * Time.deltaTime;
            flashlightPower = Mathf.Clamp(flashlightPower, 0, maxFlashlightPower);
        }

        // Disable automatically when empty
        if (flashlight2D != null)
            flashlight2D.enabled = flashlightOn && flashlightPower > 0;
    }

    public void OnToggleFlashlightButton()
    {
        ToggleFlashlight(!flashlightOn);
    }

    public void ToggleFlashlight(bool state)
    {
        if (state && flashlightPower <= 0f)
        {
            flashlightOn = false;
            if (flashlight2D != null) flashlight2D.enabled = false;
            Debug.Log("Flashlight battery empty!");
            return;
        }

        flashlightOn = state;
        if (flashlight2D != null) flashlight2D.enabled = state;
    }

    public void RechargeFlashlight(float amount)
    {
        flashlightPower = Mathf.Clamp(flashlightPower + amount, 0, maxFlashlightPower);
    }


    // ------------------------------------------------------------
    // UI Update
    // ------------------------------------------------------------
    private void UpdateUI()
    {
        if (healthSlider != null)
            healthSlider.value = health;

        if (energySlider != null)
            energySlider.value = energy;

        if (flashlightText != null)
            flashlightText.text = $"Flashlight: {flashlightPower:F0}/{maxFlashlightPower}";
    }

    public void InitScene(
        Light2D sceneFlashlight,
        TMP_Text sceneFlashlightText,
        Slider sceneHealthSlider,
        Slider sceneEnergySlider,
        Image sceneDamageOverlay,
        GameObject sceneDeathScreen)
    {
        flashlight2D = sceneFlashlight;
        flashlightText = sceneFlashlightText;
        healthSlider = sceneHealthSlider;
        energySlider = sceneEnergySlider;
        damageOverlay = sceneDamageOverlay;
        deathScreen = sceneDeathScreen;
    }


    // ------------------------------------------------------------
    // Inventory & Hotbar
    // ------------------------------------------------------------
    public void AddItem(ItemData newItem)
    {
        if (inventory.Count >= maxInventorySize)
        {
            Debug.Log("Inventory full!");
            return;
        }

        inventory.Add(newItem);
        RefreshHotbar();
    }

    public void UseItem(ItemData item)
    {
        switch (item.type)
        {
            case ItemPickup.ItemType.Heal: Heal(item.value); break;
            case ItemPickup.ItemType.Energy: energy = Mathf.Clamp(energy + item.value, 0, maxEnergy); break;
            case ItemPickup.ItemType.Baterry: flashlightPower = Mathf.Clamp(flashlightPower + item.value, 0, maxFlashlightPower); break;
        }

        inventory.Remove(item);
        RefreshHotbar();
    }

    public void InitHotbarUI()
    {
        if (slots == null || slots.Length == 0)
        {
            Debug.LogWarning("No slots assigned!");
            return;
        }

        slotButtons = new Button[slots.Length];
        slotIcons = new Image[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            slotButtons[i] = slots[i].GetComponent<Button>();
            slotIcons[i] = slots[i].transform.Find("Icon").GetComponent<Image>();

            int index = i;
            slotButtons[i].onClick.RemoveAllListeners();
            slotButtons[i].onClick.AddListener(() => UseHotbarItem(index));
        }

        RefreshHotbar();
    }

    private void UseHotbarItem(int index)
    {
        if (index < inventory.Count)
            UseItem(inventory[index]);
    }

    public void RefreshHotbar()
    {
        if (slotIcons == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Count)
            {
                slotIcons[i].sprite = inventory[i].icon;
                slotIcons[i].color = Color.white;
            }
            else
            {
                slotIcons[i].sprite = null;
                slotIcons[i].color = new Color(1, 1, 1, 0);
            }
        }
    }


    // ------------------------------------------------------------
    // Door Lock System
    // ------------------------------------------------------------
    public void LockDoor(string doorID)
    {
        lockedDoors.Add(doorID);
    }

    public bool IsDoorLocked(string doorID)
    {
        return lockedDoors.Contains(doorID);
    }
}


// ------------------------------------------------------------
// Item Data Structure
// ------------------------------------------------------------
[System.Serializable]
public class ItemData
{
    public string itemName;
    public Sprite icon;
    public ItemPickup.ItemType type;
    public int value;

    public ItemData(string name, Sprite ic, ItemPickup.ItemType t, int v)
    {
        itemName = name;
        icon = ic;
        type = t;
        value = v;
    }
}
