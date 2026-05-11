using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightSystem : MonoBehaviour
{
    public event Action<bool, float, float> OnFlashlightChanged;

    public bool IsOn { get; private set; }
    public float CurrentPower { get; private set; }
    public float MaxPower { get; private set; } = 100f;

    private float drainRate = 10f;
    private AudioSource audioSource;
    private AudioClip toggleClip;
    private Light2D boundLight;

    private void Awake()
    {
        Services.Flashlight = this;
    }

    public void Configure(
        bool isOn,
        float currentPower,
        float maxPower,
        float powerDrainRate,
        AudioSource sceneAudioSource,
        AudioClip flashlightToggleClip)
    {
        IsOn = isOn;
        MaxPower = Mathf.Max(1f, maxPower);
        CurrentPower = Mathf.Clamp(currentPower, 0f, MaxPower);
        drainRate = Mathf.Max(0f, powerDrainRate);
        audioSource = sceneAudioSource;
        toggleClip = flashlightToggleClip;
        ApplyLightState();
        PublishState();
    }

    public void BindLight(Light2D sceneLight)
    {
        boundLight = sceneLight;
        ApplyLightState();
    }

    public void UnbindLight(Light2D sceneLight)
    {
        if (boundLight == sceneLight)
        {
            boundLight = null;
        }
    }

    public void ToggleWithSound()
    {
        if (audioSource != null && toggleClip != null)
        {
            audioSource.PlayOneShot(toggleClip);
        }

        Toggle(!IsOn);
    }

    public void Toggle(bool state)
    {
        if (state && CurrentPower <= 0f)
        {
            state = false;
        }

        if (IsOn == state && !(IsOn && CurrentPower <= 0f))
        {
            ApplyLightState();
            PublishState();
            return;
        }

        IsOn = state;
        ApplyLightState();
        PublishState();
    }

    public void Recharge(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        SetCurrentPower(CurrentPower + amount);
    }

    public void SetCurrentPower(float value)
    {
        float clamped = Mathf.Clamp(value, 0f, MaxPower);
        if (Mathf.Approximately(clamped, CurrentPower))
        {
            ApplyLightState();
            PublishState();
            return;
        }

        CurrentPower = clamped;
        if (CurrentPower <= 0f)
        {
            IsOn = false;
        }

        ApplyLightState();
        PublishState();
    }

    public void ResetState()
    {
        CurrentPower = MaxPower;
        IsOn = false;
        ApplyLightState();
        PublishState();
    }

    private void Update()
    {
        if (!IsOn || CurrentPower <= 0f)
        {
            ApplyLightState();
            return;
        }

        SetCurrentPower(CurrentPower - (drainRate * Time.deltaTime));
    }

    private void ApplyLightState()
    {
        if (boundLight != null)
        {
            boundLight.enabled = IsOn && CurrentPower > 0f;
        }
    }

    private void PublishState()
    {
        OnFlashlightChanged?.Invoke(IsOn, CurrentPower, MaxPower);
    }
}
