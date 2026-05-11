using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider serializedHealthSlider;
    [SerializeField] private Image serializedDamageOverlay;
    [SerializeField] private float serializedOverlayDuration = 0.5f;
    [SerializeField] private float serializedOverlayMaxAlpha = 0.5f;

    private Slider healthSlider;
    private Image damageOverlay;
    private float overlayDuration = 0.5f;
    private float overlayMaxAlpha = 0.5f;
    private Coroutine overlayCoroutine;
    private bool subscribed;
    private bool initialized;

    public void Initialize(Slider slider, Image overlay, float duration, float maxAlpha)
    {
        healthSlider = slider;
        damageOverlay = overlay;
        overlayDuration = Mathf.Max(0.01f, duration);
        overlayMaxAlpha = Mathf.Clamp01(maxAlpha);
        initialized = true;

        if (damageOverlay != null)
        {
            Color color = damageOverlay.color;
            color.a = 0f;
            damageOverlay.color = color;
        }

        Subscribe();
        Refresh(Services.Health != null ? Services.Health.CurrentHealth : 0, Services.Health != null ? Services.Health.MaxHealth : 1);
    }

    private void Start()
    {
        if (!initialized)
        {
            Initialize(serializedHealthSlider, serializedDamageOverlay, serializedOverlayDuration, serializedOverlayMaxAlpha);
        }
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        if (!subscribed || Services.Health == null)
        {
            return;
        }

        Services.Health.OnHealthChanged -= Refresh;
        Services.Health.OnDamageTaken -= FlashOverlay;
        Services.Health.OnRevived -= ClearOverlay;
        subscribed = false;
    }

    private void Subscribe()
    {
        if (subscribed || Services.Health == null)
        {
            return;
        }

        Services.Health.OnHealthChanged += Refresh;
        Services.Health.OnDamageTaken += FlashOverlay;
        Services.Health.OnRevived += ClearOverlay;
        subscribed = true;
    }

    private void Refresh(int current, int max)
    {
        if (healthSlider == null)
        {
            return;
        }

        healthSlider.maxValue = max;
        healthSlider.value = current;
    }

    private void FlashOverlay(int damage)
    {
        if (overlayCoroutine != null)
        {
            StopCoroutine(overlayCoroutine);
        }

        overlayCoroutine = StartCoroutine(FlashOverlayRoutine());
    }

    private IEnumerator FlashOverlayRoutine()
    {
        if (damageOverlay == null)
        {
            yield break;
        }

        Color color = damageOverlay.color;
        float halfDuration = overlayDuration / 2f;
        float timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, overlayMaxAlpha, timer / halfDuration);
            damageOverlay.color = color;
            yield return null;
        }

        timer = 0f;
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(overlayMaxAlpha, 0f, timer / halfDuration);
            damageOverlay.color = color;
            yield return null;
        }

        color.a = 0f;
        damageOverlay.color = color;
    }

    private void ClearOverlay()
    {
        if (damageOverlay == null)
        {
            return;
        }

        Color color = damageOverlay.color;
        color.a = 0f;
        damageOverlay.color = color;
    }
}
