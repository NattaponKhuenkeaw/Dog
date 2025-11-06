using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    public DoorClick doorClick;
    private PlayerInput playerInput;
    private CapsuleCollider2D col;
    private SpriteRenderer spriteRenderer;

    public bool playerIsNearDoor = false;
    public bool playerIsNearHide = false;
    public bool stopX = false;

    [Header("Hiding Settings")]
    public bool isHidden = false;
    public float damageRate = 5f;
    public float safeHideTime = 5f;
    private Coroutine damageCoroutine;
    public Image hideImage;

    [Header("Warning Image")]
    public Image warningImage;        // 🔹 รูปภาพเตือน
    public float fadeInSpeed = 2f;
    public float fadeOutSpeed = 5f;
    
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    private float currentSpeed;

    [Header("Energy Settings")]
    public bool useEnergySystem = true;
    public float runEnergyCost = 3f;

    void Start()
    {
        hideImage.gameObject.SetActive(false);
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();

        if (warningImage != null)
        {
            // ตั้งให้โปร่งใสตอนเริ่ม
            Color color = warningImage.color;
            color.a = 0f;
            warningImage.color = color;
        }
    }

    void Update()
    {
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        float x = input.x;
        float y = input.y;
        float inputMagnitude = Mathf.Abs(x);
        float currentEnergy = GameManager.instance.energy;

        // 🔹 กำหนดความเร็ว
        if (inputMagnitude > 0.1f && inputMagnitude <= 0.6f)
        {
            currentSpeed = walkSpeed;
            GameManager.instance.isRunning = false;
        }
        else if (inputMagnitude > 0.6f)
        {
            currentSpeed = currentEnergy > 0 ? runSpeed : walkSpeed;
            GameManager.instance.isRunning = currentSpeed == runSpeed;
        }
        else
        {
            currentSpeed = 0f;
            GameManager.instance.isRunning = false;
        }

        // 🔹 เข้าประตู
        if (doorClick != null && y > 0.8f && playerIsNearDoor)
            doorClick.OpenDoor();

        // 🔹 ซ่อนตัว
        if (playerIsNearHide && y > 0.8f && !isHidden)
            StartHiding();
        else if (isHidden && y < -0.8f)
            StopHiding();

        // 🔹 เคลื่อนที่
        if (isHidden || stopX)
            x = 0f;

        Vector3 move = new Vector3(x, 0, 0);
        transform.position += move * currentSpeed * Time.deltaTime;

        // ส่งสถานะ
        GameManager.instance.isMoving = currentSpeed > 0;

        // 🔹 ใช้พลังงานตอนวิ่ง
        if (useEnergySystem && GameManager.instance.isRunning && currentEnergy > 0)
            GameManager.instance.UseEnergy(runEnergyCost * Time.deltaTime);
    }

    // -------------------------------
    // 🔸 ฟังก์ชันซ่อน / ออกจากที่ซ่อน
    // -------------------------------
    void StartHiding()
    {
        isHidden = true;
        spriteRenderer.enabled = false;
        col.enabled = false;
        hideImage.gameObject.SetActive(true);

        Debug.Log("เริ่มซ่อนตัว");

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        damageCoroutine = StartCoroutine(HideDamageRoutine());
    }

    void StopHiding()
    {
        isHidden = false;
        spriteRenderer.enabled = true;
        col.enabled = true;
        hideImage.gameObject.SetActive(false);

        Debug.Log("ออกจากการซ่อน");

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        // 🔹 เมื่อออกจากที่ซ่อนให้ภาพเตือนหายด้วย
        if (warningImage != null)
            StartCoroutine(FadeImage(0f));
    }

    // -------------------------------
    // 🔸 Coroutine ทำดาเมจหลังซ่อนเกินเวลา
    // -------------------------------
    IEnumerator HideDamageRoutine()
    {
        float halfTime = safeHideTime / 2f;

        // 🔹 รอครึ่งเวลาถึงจะแสดงภาพเตือน
        yield return new WaitForSeconds(halfTime);

        if (isHidden && warningImage != null)
        {
            Debug.Log("⚠️ เริ่มแสดงภาพเตือน");
            StartCoroutine(FadeImage(1f)); // ค่อย ๆ เฟดขึ้น
        }

        // 🔹 รออีกครึ่งเวลาจนหมด
        yield return new WaitForSeconds(halfTime);

        if (isHidden)
        {
            GameManager.instance.TakeDamage((int)damageRate);
            Debug.Log($"⛔ ซ่อนนานเกินไป เสียเลือด {damageRate}");

            // ค่อย ๆ เฟดภาพเตือนหายไป
            if (warningImage != null)
                StartCoroutine(FadeImage(0f));

            StopHiding();
        }
    }

    IEnumerator FadeImage(float targetAlpha)
    {
        float speed = targetAlpha > 0 ? fadeInSpeed : fadeOutSpeed; // ถ้าเฟดเข้า ใช้ fadeInSpeed

        Color color = warningImage.color;
        while (!Mathf.Approximately(color.a, targetAlpha))
        {
            color.a = Mathf.MoveTowards(color.a, targetAlpha, Time.deltaTime * speed);
            warningImage.color = color;
            yield return null;
        }
    }


    // -------------------------------
    // 🔸 ตรวจจับพื้นที่
    // -------------------------------
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            playerIsNearDoor = true;
        }
        else if (other.CompareTag("HideSpot"))
        {
            playerIsNearHide = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            playerIsNearDoor = false;
        }
        else if (other.CompareTag("HideSpot"))
        {
            playerIsNearHide = false;
        }
    }
}
