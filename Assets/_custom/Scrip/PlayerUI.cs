using UnityEngine;
using UnityEngine.UI;
using TMPro;  

public class PlayerUI : MonoBehaviour
{
    public Slider healthSlider;
    public Slider energySlider;

    [Header("Flashlight UI")]
    public TMP_Text flashlightText; 

    void Update()
    {
        if (GameManager.instance != null)
        {
            // แถบเลือดและพลังงาน
            healthSlider.value = GameManager.instance.health;
            energySlider.value = GameManager.instance.energy;

            // แสดงค่าพลังไฟฉายเป็นตัวเลข
            float current = GameManager.instance.flashlightPower;
            float max = GameManager.instance.maxFlashlightPower;
            flashlightText.text = $"Flashlight: {current:F0} / {max}";
        }
    }
}
