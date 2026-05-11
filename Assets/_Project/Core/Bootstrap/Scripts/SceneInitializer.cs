using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Video;

public class SceneInitializer : MonoBehaviour
{
    [Header("Scene References")]
    public Light2D flashlight2D;
    public TMP_Text flashlightText;
    public Slider healthSlider;
    public Slider energySlider;
    public Button flashlightButton;
    public Image damageOverlay;
    public GameObject deathScreen;

    [Header("Death Video References")]
    public VideoPlayer deathVideoPlayer;
    public GameObject videoRawImage;

    [Header("Hotbar UI (3 Slots)")]
    public GameObject[] hotbarSlots;

    private void Start()
    {
        Services.Session?.TryRestoreTaggedPlayerPosition();

        if (!HasDirectSceneBinders())
        {
            BindSceneServices();
        }
    }

    private bool HasDirectSceneBinders()
    {
        return HasExternalBinder<HealthUI>()
            || HasExternalBinder<EnergyUI>()
            || HasExternalBinder<FlashlightUI>()
            || HasExternalBinder<DeathSequenceUI>()
            || HasExternalBinder<InventoryHotbarUI>();
    }

    private bool HasExternalBinder<T>() where T : Component
    {
        T[] binders = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        for (int i = 0; i < binders.Length; i++)
        {
            if (binders[i] != null && binders[i].gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private void BindSceneServices()
    {
        GameManager manager = GameManager.instance;
        if (manager != null)
        {
            manager.InitScene(
                flashlight2D,
                flashlightText,
                healthSlider,
                energySlider,
                damageOverlay,
                deathScreen,
                deathVideoPlayer,
                videoRawImage);
        }

        HealthUI healthUI = GetOrAddComponent<HealthUI>();
        healthUI.Initialize(
            healthSlider,
            damageOverlay,
            manager != null ? manager.overlayDuration : 0.5f,
            manager != null ? manager.overlayMaxAlpha : 0.5f);

        EnergyUI energyUIComponent = GetOrAddComponent<EnergyUI>();
        energyUIComponent.Initialize(energySlider);

        FlashlightUI flashlightUIComponent = GetOrAddComponent<FlashlightUI>();
        flashlightUIComponent.Initialize(flashlightText, flashlight2D);

        DeathSequenceUI deathUI = GetOrAddComponent<DeathSequenceUI>();
        deathUI.Initialize(deathScreen, deathVideoPlayer, videoRawImage);

        InventoryHotbarUI hotbarUI = GetOrAddComponent<InventoryHotbarUI>();
        hotbarUI.Initialize(hotbarSlots);
        if (manager != null)
        {
            manager.slots = hotbarSlots;
            manager.RegisterHotbarUI(hotbarUI);
        }

        if (flashlightButton != null)
        {
            flashlightButton.onClick.RemoveAllListeners();
            flashlightButton.onClick.AddListener(() => Services.Flashlight?.ToggleWithSound());
        }
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
