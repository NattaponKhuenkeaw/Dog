using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Burst.Intrinsics.X86.Sse4_2;

public class Player : MonoBehaviour
{
    public DoorClick doorClick;
    private PlayerInput playerInput;
    private CapsuleCollider2D col;


    public bool playerIsNearDoor = false;
    public bool playerIsNearHide = false;

    public bool stopX = false;



    [Header("Hiding Settings")]
    public bool isHidden = false;        // ผู้เล่นซ่อนอยู่หรือไม่
    public float damageRate = 5f;        // เสียเลือดต่อรอบ
    public float damageInterval = 2f;    // ทุกกี่วินาทีจะเสียเลือด
    public float safeHideTime = 5f;      // ซ่อนได้ฟรีกี่วินาที (ไม่เสียเลือด)
    private Coroutine damageCoroutine;
    private SpriteRenderer spriteRenderer;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    private float currentSpeed;

    [Header("Energy Settings")]
    public bool useEnergySystem = true;
    public float runEnergyCost = 3f; // ใช้พลังต่อวินาทีตอนวิ่ง

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
        // เข้าประตู
        if (doorClick != null && y > 0.8f && playerIsNearDoor)
        {
            doorClick.OpenDoor();
        }







        // ซ่อนตัว
        if (playerIsNearHide && y > 0.8f )
        {
             
            isHidden = true;
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }
            col.enabled = false;
      
        }
        else if (isHidden && y < -0.8f)
        {
            
            isHidden = false;           
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }
            col.enabled = true;

        }


        // เคลื่อนที่
        if (isHidden || stopX)
        {
            x = 0f;
            //FindObjectOfType<Player>().stopX = false;  // กลับมาขยับได้
            //FindObjectOfType<Player>().stopX = true;   // หยุดแกน X
        }
        Vector3 move = new Vector3(x, 0, 0);
        transform.position += move * currentSpeed * Time.deltaTime;

        // ส่งสถานะการเคลื่อนไหว
        GameManager.instance.isMoving = currentSpeed > 0;

        // ใช้พลังงานตอนวิ่ง
        if (useEnergySystem && GameManager.instance.isRunning && currentEnergy > 0)
        {
            GameManager.instance.UseEnergy(runEnergyCost * Time.deltaTime);
        }
    }

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
