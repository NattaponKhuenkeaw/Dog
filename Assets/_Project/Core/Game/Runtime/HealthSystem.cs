using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnDamageTaken;
    public event Action OnDeath;
    public event Action OnRevived;

    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; } = 100;
    public bool IsDead { get; private set; }

    private AudioSource damageAudioSource;
    private AudioClip damageSound;

    private void Awake()
    {
        Services.Health = this;
    }

    public void Configure(int currentHealth, int maxHealth, AudioSource audioSource, AudioClip clip)
    {
        MaxHealth = Mathf.Max(1, maxHealth);
        damageAudioSource = audioSource;
        damageSound = clip;
        CurrentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        IsDead = CurrentHealth <= 0;
        PublishState();
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || IsDead)
        {
            return;
        }

        int previousHealth = CurrentHealth;
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);

        if (CurrentHealth == previousHealth)
        {
            return;
        }

        if (damageAudioSource != null && damageSound != null)
        {
            damageAudioSource.PlayOneShot(damageSound);
        }

        OnDamageTaken?.Invoke(amount);
        PublishState();

        if (CurrentHealth <= 0 && !IsDead)
        {
            IsDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        SetCurrentHealth(CurrentHealth + amount);
    }

    public void SetCurrentHealth(int value)
    {
        int clamped = Mathf.Clamp(value, 0, MaxHealth);
        bool wasDead = IsDead;

        if (CurrentHealth == clamped && (clamped <= 0) == wasDead)
        {
            return;
        }

        CurrentHealth = clamped;
        IsDead = CurrentHealth <= 0;
        PublishState();

        if (!wasDead && IsDead)
        {
            OnDeath?.Invoke();
        }
        else if (wasDead && !IsDead)
        {
            OnRevived?.Invoke();
        }
    }

    public void ResetState()
    {
        bool wasDead = IsDead;
        CurrentHealth = MaxHealth;
        IsDead = false;
        PublishState();

        if (wasDead)
        {
            OnRevived?.Invoke();
        }
    }

    private void PublishState()
    {
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }
}
