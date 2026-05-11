using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class FlashlightUI : MonoBehaviour
{
    [SerializeField] private TMP_Text serializedFlashlightText;
    [SerializeField] private Light2D serializedFlashlightLight;
    [SerializeField] private Button serializedFlashlightButton;

    private TMP_Text flashlightText;
    private Light2D flashlightLight;
    private bool subscribed;
    private bool initialized;

    public void Initialize(TMP_Text label, Light2D sceneLight)
    {
        flashlightText = label;
        flashlightLight = sceneLight;
        initialized = true;

        if (flashlightLight != null)
        {
            Services.Flashlight?.BindLight(flashlightLight);
        }

        if (serializedFlashlightButton != null)
        {
            serializedFlashlightButton.onClick.RemoveAllListeners();
            serializedFlashlightButton.onClick.AddListener(() => Services.Flashlight?.ToggleWithSound());
        }

        Subscribe();
        Refresh(
            Services.Flashlight != null && Services.Flashlight.IsOn,
            Services.Flashlight != null ? Services.Flashlight.CurrentPower : 0f,
            Services.Flashlight != null ? Services.Flashlight.MaxPower : 1f);
    }

    private void Start()
    {
        if (!initialized)
        {
            Initialize(serializedFlashlightText, serializedFlashlightLight);
        }
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        if (flashlightLight != null)
        {
            Services.Flashlight?.UnbindLight(flashlightLight);
        }

        if (!subscribed || Services.Flashlight == null)
        {
            return;
        }

        Services.Flashlight.OnFlashlightChanged -= Refresh;
        subscribed = false;
    }

    private void Subscribe()
    {
        if (subscribed || Services.Flashlight == null)
        {
            return;
        }

        Services.Flashlight.OnFlashlightChanged += Refresh;
        subscribed = true;
    }

    private void Refresh(bool isOn, float current, float max)
    {
        if (flashlightText != null)
        {
            flashlightText.text = $"Flashlight: {current:F0}/{max:F0}";
        }
    }
}
