using UnityEngine;
using System.Collections;

public class HideSpot : MonoBehaviour
{
    [Header("Hiding Settings")]
    public bool isHidden = false;        // ผู้เล่นซ่อนอยู่หรือไม่
    public float damageRate = 5f;        // เสียเลือดต่อรอบ
    public float damageInterval = 2f;    // ทุกกี่วินาทีจะเสียเลือด
    public float safeHideTime = 5f;      // ซ่อนได้ฟรีกี่วินาที (ไม่เสียเลือด)
    private Coroutine damageCoroutine;
    private SpriteRenderer spriteRenderer;

    public bool playerIsNear = false;   // ผู้เล่นอยู่ใกล้จุดซ่อนหรือไม่

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetHidden(bool hidden)
    {
        isHidden = hidden;

        // 🔹 เปลี่ยนความโปร่งของตัวละครตอนซ่อน
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = hidden ? 0.5f : 1f;
            spriteRenderer.color = color;
        }

        // 🔹 เริ่มหรือหยุด coroutine ตามสถานะ
        if (hidden)
        {
            if (damageCoroutine == null)
                damageCoroutine = StartCoroutine(HideAndLoseHealth());
        }
        else
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }

        Debug.Log(hidden ? "🕵️‍♂️ Player is hiding" : "🚶 Player is visible again");
    }

    private IEnumerator HideAndLoseHealth()
    {
        // ⏳ 1️⃣ รอช่วงเวลาฟรีก่อนเริ่มเสียเลือด
        Debug.Log($"😶 Player started hiding — safe for {safeHideTime} seconds.");
        yield return new WaitForSeconds(safeHideTime);

        // 🔥 2️⃣ เริ่มเสียเลือดหลังจากเลย safeHideTime
        while (isHidden)
        {
            yield return new WaitForSeconds(damageInterval);

            if (GameManager.instance != null)
            {
                GameManager.instance.health -= (int)damageRate;
                Debug.Log($"💔 Hiding too long! Health: {GameManager.instance.health}");

              
            }
        }
    }
}