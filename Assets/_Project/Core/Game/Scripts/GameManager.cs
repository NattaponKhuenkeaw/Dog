using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip flashlightSound;

    [Header("Player State")]
    [FormerlySerializedAs("lastPlayerPosition")]
    [SerializeField] private Vector3 serializedLastPlayerPosition;
    [FormerlySerializedAs("health")]
    [SerializeField] private int startingHealth = 100;
    public int maxHealth = 100;
    [FormerlySerializedAs("energy")]
    [SerializeField] private float startingEnergy = 100f;
    public float maxEnergy = 100f;
    public GameObject deathScreen;

    [Header("Death Video Settings")]
    public VideoPlayer deathVideoPlayer;
    public GameObject videoRawImage;

    [Header("Damage Overlay")]
    public Image damageOverlay;
    public float overlayDuration = 0.5f;
    public float overlayMaxAlpha = 0.5f;

    [Header("Energy Regeneration")]
    public bool energyRegenEnabled = true;
    public float walkRegenRate = 2f;
    public float idleRegenRate = 5f;
    public float energyRegenDelay = 1.5f;

    [Header("Flashlight System")]
    [FormerlySerializedAs("flashlightOn")]
    [SerializeField] private bool startingFlashlightOn;
    [FormerlySerializedAs("flashlightPower")]
    [SerializeField] private float startingFlashlightPower = 100f;
    public float maxFlashlightPower = 100f;
    public float flashlightDrainRate = 10f;
    public Light2D flashlight2D;
    public TMP_Text flashlightText;

    [Header("UI Sliders")]
    public Slider healthSlider;
    public Slider energySlider;

    [Header("Movement Flags")]
    public bool isMoving;
    public bool isRunning;

    [Header("Inventory System")]
    public int maxInventorySize = 3;

    [Header("Hotbar UI (3 Slots)")]
    public GameObject[] slots = new GameObject[3];

    private readonly List<ItemData> legacyInventoryFallback = new List<ItemData>();
    private InventoryHotbarUI hotbarUI;
    private SessionManager sessionManager;
    private HealthSystem healthSystem;
    private EnergySystem energySystem;
    private FlashlightSystem flashlightSystem;
    private InventoryManager inventoryManager;
    private DoorLockRegistry doorLockRegistry;
    private SceneLoader sceneLoader;

    public Vector3 lastPlayerPosition
    {
        get => Services.Session != null ? Services.Session.LastPlayerPosition : serializedLastPlayerPosition;
        set
        {
            serializedLastPlayerPosition = value;
            if (Services.Session != null)
            {
                Services.Session.LastPlayerPosition = value;
            }
        }
    }

    public bool isDead
    {
        get => Services.Health != null && Services.Health.IsDead;
        set
        {
            if (Services.Health == null)
            {
                return;
            }

            if (value)
            {
                Services.Health.SetCurrentHealth(0);
            }
            else
            {
                Services.Health.ResetState();
            }
        }
    }

    public int health
    {
        get => Services.Health != null ? Services.Health.CurrentHealth : startingHealth;
        set
        {
            startingHealth = value;
            Services.Health?.SetCurrentHealth(value);
        }
    }

    public float energy
    {
        get => Services.Energy != null ? Services.Energy.CurrentEnergy : startingEnergy;
        set
        {
            startingEnergy = value;
            Services.Energy?.SetCurrentEnergy(value);
        }
    }

    public bool flashlightOn
    {
        get => Services.Flashlight != null && Services.Flashlight.IsOn;
        set
        {
            startingFlashlightOn = value;
            Services.Flashlight?.Toggle(value);
        }
    }

    public float flashlightPower
    {
        get => Services.Flashlight != null ? Services.Flashlight.CurrentPower : startingFlashlightPower;
        set
        {
            startingFlashlightPower = value;
            Services.Flashlight?.SetCurrentPower(value);
        }
    }

    public List<ItemData> inventory => Services.Inventory != null ? Services.Inventory.Items : legacyInventoryFallback;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        gameObject.name = "[Services]";
        DontDestroyOnLoad(gameObject);

        EnsureServices();
        ConfigureServices();
    }

    private void EnsureServices()
    {
        sessionManager = GetOrAddComponent<SessionManager>();
        healthSystem = GetOrAddComponent<HealthSystem>();
        energySystem = GetOrAddComponent<EnergySystem>();
        flashlightSystem = GetOrAddComponent<FlashlightSystem>();
        inventoryManager = GetOrAddComponent<InventoryManager>();
        doorLockRegistry = GetOrAddComponent<DoorLockRegistry>();
        sceneLoader = GetOrAddComponent<SceneLoader>();

        Services.Session = sessionManager;
        Services.Health = healthSystem;
        Services.Energy = energySystem;
        Services.Flashlight = flashlightSystem;
        Services.Inventory = inventoryManager;
        Services.Doors = doorLockRegistry;
        Services.SceneLoader = sceneLoader;
    }

    private void ConfigureServices()
    {
        sessionManager.LastPlayerPosition = serializedLastPlayerPosition;
        healthSystem.Configure(startingHealth, maxHealth, audioSource, damageSound);
        energySystem.Configure(
            startingEnergy,
            maxEnergy,
            energyRegenEnabled,
            walkRegenRate,
            idleRegenRate,
            energyRegenDelay);
        flashlightSystem.Configure(
            startingFlashlightOn,
            startingFlashlightPower,
            maxFlashlightPower,
            flashlightDrainRate,
            audioSource,
            flashlightSound);
        inventoryManager.Configure(maxInventorySize);
    }

    public void TakeDamage(int damage)
    {
        Services.Health?.TakeDamage(damage);
    }

    public void Heal(int amount)
    {
        Services.Health?.Heal(amount);
    }

    public void UseEnergy(float amount)
    {
        Services.Energy?.UseEnergy(amount);
    }

    public void OnToggleFlashlightButton()
    {
        Services.Flashlight?.ToggleWithSound();
    }

    public void ToggleFlashlight(bool state)
    {
        Services.Flashlight?.Toggle(state);
    }

    public void RechargeFlashlight(float amount)
    {
        Services.Flashlight?.Recharge(amount);
    }

    public void InitScene(
        Light2D sceneFlashlight,
        TMP_Text sceneFlashlightText,
        Slider sceneHealthSlider,
        Slider sceneEnergySlider,
        Image sceneDamageOverlay,
        GameObject sceneDeathScreen,
        VideoPlayer sceneVideoPlayer,
        GameObject sceneVideoRawImage)
    {
        flashlight2D = sceneFlashlight;
        flashlightText = sceneFlashlightText;
        healthSlider = sceneHealthSlider;
        energySlider = sceneEnergySlider;
        damageOverlay = sceneDamageOverlay;
        deathScreen = sceneDeathScreen;
        deathVideoPlayer = sceneVideoPlayer;
        videoRawImage = sceneVideoRawImage;

        if (sceneFlashlight != null)
        {
            Services.Flashlight?.BindLight(sceneFlashlight);
        }
    }

    public bool AddItem(ItemData newItem)
    {
        return Services.Inventory != null && Services.Inventory.AddItem(newItem);
    }

    public void UseItem(ItemData item)
    {
        Services.Inventory?.UseItem(item);
    }

    public void RegisterHotbarUI(InventoryHotbarUI ui)
    {
        hotbarUI = ui;
    }

    public void InitHotbarUI()
    {
        hotbarUI?.Initialize(slots);
    }

    public void RefreshHotbar()
    {
        if (hotbarUI != null)
        {
            hotbarUI.Refresh();
        }
        else
        {
            Services.Inventory?.NotifyInventoryChanged();
        }
    }

    public void LockDoor(string doorID)
    {
        Services.Doors?.LockDoor(doorID);
    }

    public bool IsDoorLocked(string doorID)
    {
        return Services.Doors != null && Services.Doors.IsDoorLocked(doorID);
    }

    public void ResetGameState()
    {
        Services.Session?.ResetGameState();
    }

    private T GetOrAddComponent<T>() where T : Component
    {
        T component = GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }
}
