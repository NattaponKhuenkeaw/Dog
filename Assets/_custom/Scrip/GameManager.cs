using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Collections;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [Header("Damage Overlay")]
    public Image damageOverlay;
    public float overlayDuration = 0.5f;  // เวลาที่หน้าจอแดง
    public float overlayMaxAlpha = 0.5f;  // ความเข้มสูงสุดของสีแดง
    private Coroutine overlayCoroutine;

    [Header("Player Stats")]
    public int health = 100;
    public float energy = 100f;
    public int maxHealth = 100;
    public float maxEnergy = 100f;

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
    public Light2D flashlight2D;            // ใส่ใน Scene ที่มีไฟฉาย
    public TMP_Text flashlightText;     // ใส่ใน Scene ที่มี UI

    [Header("UI Sliders")]
    public Slider healthSlider;
    public Slider energySlider;

    [Header("Movement Flags")]
    public bool isMoving = false;
    public bool isRunning = false;

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

    private void Update()
    {
        HandleEnergySystem();
        HandleFlashlight();
        UpdateUI();
    }

    // ---------------------------- //
    //      ระบบ HP/พลังงาน
    // ---------------------------- //
    public void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);

        if (overlayCoroutine != null)
            StopCoroutine(overlayCoroutine);
        overlayCoroutine = StartCoroutine(FlashDamageOverlay());
    }

    private IEnumerator FlashDamageOverlay()
    {
        if (damageOverlay == null)
            yield break;

        // เริ่มจากโปร่งใส -> แดง -> โปร่งใส
        Color color = damageOverlay.color;

        // เฟดเข้า
        float t = 0f;
        while (t < overlayDuration / 2)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, overlayMaxAlpha, t / (overlayDuration / 2));
            damageOverlay.color = color;
            yield return null;
        }

        // เฟดออก
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
        regenTimer = 0f; // รีเซ็ตเวลา regen ทุกครั้งที่ใช้พลัง
    }

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
                energy += regenRate * Time.deltaTime;
                energy = Mathf.Clamp(energy, 0, maxEnergy);
            }
        }
    }

    // ---------------------------- //
    //        ระบบไฟฉาย
    // ---------------------------- //
    private void HandleFlashlight()
    {
        if (flashlightOn && flashlight2D != null && flashlightPower > 0f)
        {
            flashlightPower -= flashlightDrainRate * Time.deltaTime;
            flashlightPower = Mathf.Clamp(flashlightPower, 0f, maxFlashlightPower);

            flashlight2D.enabled = flashlightPower > 0;

            if (flashlightPower <= 0f)
                ToggleFlashlight(false);
        }
        else if (flashlight2D != null)
        {
            flashlight2D.enabled = false;
        }
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
        flashlightPower = Mathf.Clamp(flashlightPower + amount, 0f, maxFlashlightPower);
        Debug.Log("Flashlight recharged: " + flashlightPower);
    }

    public void OnToggleFlashlightButton()
    {
        
        ToggleFlashlight(!flashlightOn);
    }


    // ---------------------------- //
    //         ระบบ UI
    // ---------------------------- //
    private void UpdateUI()
    {
        if (healthSlider != null)
            healthSlider.value = health;

        if (energySlider != null)
            energySlider.value = energy;

        if (flashlightText != null)
        {
            flashlightText.text = $"Flashlight: {flashlightPower:F0} / {maxFlashlightPower}";
        }
    }

    // ---------------------------- //
    //       Scene Initialization
    // ---------------------------- //
    public void InitScene(
    Light2D sceneFlashlight = null,
    TMP_Text sceneFlashlightText = null,
    Slider sceneHealthSlider = null,
    Slider sceneEnergySlider = null,
    Image sceneDamageOverlay = null
    )
    {
        flashlight2D = sceneFlashlight;
        flashlightText = sceneFlashlightText;
        healthSlider = sceneHealthSlider;
        energySlider = sceneEnergySlider;
        damageOverlay = sceneDamageOverlay;
    }

    // ---------------------------- //
    //       เก็บของฮาฟฟู่
    // ---------------------------- //



}
