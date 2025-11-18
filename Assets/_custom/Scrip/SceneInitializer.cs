using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Collections;

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


    [Header("Hotbar UI (3 ช่อง)")]
    public GameObject[] hotbarSlots;   // ใส่ Slot1, Slot2, Slot3 ของ Scene นี้

    void Start()
    {



        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && GameManager.instance.lastPlayerPosition != Vector3.zero)
        {
            // 3. ย้าย Player ไปยังตำแหน่งที่บันทึกไว้
            player.transform.position = GameManager.instance.lastPlayerPosition;

            // 4. (สำคัญมาก) ล้างค่าที่จำไว้ ไม่งั้นทุกครั้งที่โหลด Scene นี้ Player จะวาร์ปกลับมาที่นี่ตลอด
            
        }

        GameManager.instance.deathScreen = deathScreen;

        // ------------------------------
        // ✅ 1. อัปเดต reference ให้ GameManager
        // ------------------------------
        if (GameManager.instance != null)
        {
            GameManager.instance.InitScene(
                flashlight2D,
                flashlightText,
                healthSlider,
                energySlider,
                damageOverlay,
                deathScreen
            );

            // ------------------------------
            // ✅ 2. ตั้งค่า Hotbar ใหม่ของ Scene นี้
            // ------------------------------
            if (hotbarSlots != null && hotbarSlots.Length > 0)
            {
                GameManager.instance.slots = hotbarSlots;
                GameManager.instance.InitHotbarUI();   // เรียกฟังก์ชันใน GameManager ที่เชื่อม UI
            }
            else
            {
                Debug.LogWarning("⚠️ ยังไม่ได้กำหนด HotbarSlots ใน SceneInitializer!");
            }
            
           
        }
        else
        {
            Debug.LogWarning("❌ ไม่มี GameManager ใน Scene นี้!");
        }

        // ------------------------------
        // ✅ 3. ปุ่มไฟฉาย
        // ------------------------------
        if (flashlightButton != null)
        {
            flashlightButton.onClick.RemoveAllListeners(); // ป้องกัน listener ซ้ำ
            flashlightButton.onClick.AddListener(() =>
            {
                Debug.Log("🟡 Flashlight button clicked!");
                if (GameManager.instance != null)
                    GameManager.instance.OnToggleFlashlightButton();
            });
        }
    }
        

}
