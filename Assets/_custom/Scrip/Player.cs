using UnityEngine;
using UnityEngine.InputSystem;
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
    public bool isHidden = false;        // ผู้เล่นซ่อนอยู่หรือไม่
    public float damageRate = 5f;        // เสียเลือดต่อรอบ
    public float safeHideTime = 5f;      // ซ่อนได้ฟรีกี่วินาที
    private Coroutine damageCoroutine;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    private float currentSpeed;

    [Header("Energy Settings")]
    public bool useEnergySystem = true;
    public float runEnergyCost = 3f;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();
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
        {
            doorClick.OpenDoor();
        }

        // 🔹 ซ่อนตัว
        if (playerIsNearHide && y > 0.8f && !isHidden)
        {
            StartHiding();
        }
        else if (isHidden && y < -0.8f)
        {
            StopHiding();
        }

        // 🔹 เคลื่อนที่
        if (isHidden || stopX)
        {
            x = 0f;
        }

        Vector3 move = new Vector3(x, 0, 0);
        transform.position += move * currentSpeed * Time.deltaTime;

        // ส่งสถานะ
        GameManager.instance.isMoving = currentSpeed > 0;

        // 🔹 ใช้พลังงานตอนวิ่ง
        if (useEnergySystem && GameManager.instance.isRunning && currentEnergy > 0)
        {
            GameManager.instance.UseEnergy(runEnergyCost * Time.deltaTime);
        }
    }

    // -------------------------------
    // 🔸 ฟังก์ชันซ่อน / ออกจากที่ซ่อน
    // -------------------------------
    void StartHiding()
    {
        isHidden = true;
        spriteRenderer.enabled = false;
        col.enabled = false;

        Debug.Log("เริ่มซ่อนตัว");

        // เริ่มนับเวลา Coroutine
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(HideDamageRoutine());
    }

    void StopHiding()
    {
        isHidden = false;
        spriteRenderer.enabled = true;
        col.enabled = true;

        Debug.Log("ออกจากการซ่อน");

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
    }

    // -------------------------------
    // 🔸 Coroutine ทำดาเมจหลังซ่อนเกินเวลา
    // -------------------------------
    IEnumerator HideDamageRoutine()
    {
        yield return new WaitForSeconds(safeHideTime); // ซ่อนได้ฟรีก่อน 5 วิ

        if (isHidden) // ถ้ายังซ่อนอยู่
        {
            GameManager.instance.TakeDamage((int)damageRate);
            Debug.Log($"⛔ ซ่อนนานเกินไป เสียเลือด {damageRate}");

            

            StopHiding();
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
            Debug.Log("Player เข้ามาใกล้ประตูแล้ว");
        }
        else if (other.CompareTag("HideSpot"))
        {
            playerIsNearHide = true;
            Debug.Log("Player เข้ามาใกล้จุดซ่อนแล้ว");
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            playerIsNearDoor = false;
            Debug.Log("Player ออกจากประตูแล้ว");
        }
        else if (other.CompareTag("HideSpot"))
        {
            playerIsNearHide = false;
            Debug.Log("Player ออกจากจุดซ่อนแล้ว");
        }
    }
}
