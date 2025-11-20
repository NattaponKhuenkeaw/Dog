using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.Video; // 1. อย่าลืมเพิ่มบรรทัดนี้

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

    [Header("Death Video References")] // 2. เพิ่มช่องสำหรับ Video
    public VideoPlayer deathVideoPlayer;
    public GameObject videoRawImage;

    [Header("Hotbar UI (3 ช่อง)")]
    public GameObject[] hotbarSlots;

    void Start()
    {
        // ... (โค้ดส่วนย้าย Player เหมือนเดิม) ...
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && GameManager.instance.lastPlayerPosition != Vector3.zero)
        {
            player.transform.position = GameManager.instance.lastPlayerPosition;
        }

        // ------------------------------
        // ✅ 1. อัปเดต reference ให้ GameManager
        // ------------------------------
        if (GameManager.instance != null)
        {
            // แก้ไขการเรียกใช้ InitScene โดยส่งค่า Video ไปด้วย
            GameManager.instance.InitScene(
                flashlight2D,
                flashlightText,
                healthSlider,
                energySlider,
                damageOverlay,
                deathScreen,
                deathVideoPlayer, // <-- ส่ง VideoPlayer
                videoRawImage     // <-- ส่ง RawImage GameObject
            );

            // ... (ส่วน Hotbar เหมือนเดิม) ...
            if (hotbarSlots != null && hotbarSlots.Length > 0)
            {
                GameManager.instance.slots = hotbarSlots;
                GameManager.instance.InitHotbarUI();
            }
        }
        // ... (ส่วนปุ่มไฟฉาย เหมือนเดิม) ...
        if (flashlightButton != null)
        {
            flashlightButton.onClick.RemoveAllListeners();
            flashlightButton.onClick.AddListener(() =>
            {
                if (GameManager.instance != null)
                    GameManager.instance.OnToggleFlashlightButton();
            });
        }
    }
}