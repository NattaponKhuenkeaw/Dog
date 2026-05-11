using System;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    public event Action<float, float> OnEnergyChanged;

    public float CurrentEnergy { get; private set; }
    public float MaxEnergy { get; private set; } = 100f;

    private bool regenEnabled = true;
    private float walkRegenRate = 2f;
    private float idleRegenRate = 5f;
    private float regenDelay = 1.5f;
    private float regenTimer;
    private bool isMoving;
    private bool isRunning;

    private void Awake()
    {
        Services.Energy = this;
    }

    public void Configure(
        float currentEnergy,
        float maxEnergy,
        bool enableRegen,
        float walkRate,
        float idleRate,
        float delay)
    {
        MaxEnergy = Mathf.Max(1f, maxEnergy);
        regenEnabled = enableRegen;
        walkRegenRate = walkRate;
        idleRegenRate = idleRate;
        regenDelay = Mathf.Max(0f, delay);
        CurrentEnergy = Mathf.Clamp(currentEnergy, 0f, MaxEnergy);
        PublishState();
    }

    public void SetMovementState(bool moving, bool running)
    {
        isMoving = moving;
        isRunning = running;
    }

    public void UseEnergy(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        regenTimer = 0f;
        SetCurrentEnergy(CurrentEnergy - amount);
    }

    public void AddEnergy(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        SetCurrentEnergy(CurrentEnergy + amount);
    }

    public void SetCurrentEnergy(float value)
    {
        float clamped = Mathf.Clamp(value, 0f, MaxEnergy);
        if (Mathf.Approximately(clamped, CurrentEnergy))
        {
            return;
        }

        CurrentEnergy = clamped;
        PublishState();
    }

    public void ResetState()
    {
        regenTimer = 0f;
        isMoving = false;
        isRunning = false;
        SetCurrentEnergy(MaxEnergy);
    }

    private void Update()
    {
        if (!regenEnabled || MaxEnergy <= 0f)
        {
            return;
        }

        if (isRunning)
        {
            regenTimer = 0f;
            return;
        }

        if (CurrentEnergy >= MaxEnergy)
        {
            return;
        }

        regenTimer += Time.deltaTime;
        if (regenTimer < regenDelay)
        {
            return;
        }

        float regenRate = isMoving ? walkRegenRate : idleRegenRate;
        SetCurrentEnergy(CurrentEnergy + (regenRate * Time.deltaTime));
    }

    private void PublishState()
    {
        OnEnergyChanged?.Invoke(CurrentEnergy, MaxEnergy);
    }
}
