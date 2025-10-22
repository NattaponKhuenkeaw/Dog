using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;  

public class SceneInitializer : MonoBehaviour
{
    [Header("Scene References")]
    public Light2D flashlight2D;        
    public TMP_Text flashlightText;    
    public Slider healthSlider;         
    public Slider energySlider;
    public Button flashlightButton;

    void Start()
    {
        // ถ้ามี GameManager อยู่ ให้อัปเดต reference ให้ GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.InitScene(
                flashlight2D,
                flashlightText,
                healthSlider,
                energySlider
            );
        }
        else
        {
            Debug.LogWarning("⚠️ ไม่มี GameManager ใน Scene นี้!");
        }

        if (flashlightButton != null)
        {
            flashlightButton.onClick.RemoveAllListeners(); // ป้องกัน listener ซ้ำ
            flashlightButton.onClick.AddListener(() =>
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.OnToggleFlashlightButton();
                }
            });
        }
    }
}
