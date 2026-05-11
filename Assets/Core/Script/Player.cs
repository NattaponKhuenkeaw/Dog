using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject winPanel;
    public Image hideImage;
    public Image warningImage;
    public Image jumpscareImage;

    [Header("Footstep Settings")]
    public AudioSource footstepSource;
    public AudioClip walkClip;
    public AudioClip runClip;
    public float walkInterval = 0.45f;
    public float runInterval = 0.25f;
    private float footstepTimer = 0f;

    [Header("Door System")]
    public DoorClick doorClick;

    [Header("Teleport / Interaction Flags")]
    public Transform targetPosition;
    public bool playerIsNearStairs = false;
    public bool playerIsNearDoor = false;
    public bool playerIsNearHide = false;
    public bool stopX = false;

    [Header("Hiding System")]
    public bool isHidden = false;
    public float damageRate = 5f;
    public float safeHideTime = 5f;
    private Coroutine damageCoroutine;
    public AudioSource hidingSource;
    public AudioClip hidingClip;
    public AudioClip openDoor;


    [Header("Warning Fade Settings")]
    public float fadeInSpeed = 2f;
    public float fadeOutSpeed = 5f;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    private float currentSpeed;

    [Header("Movement Boundaries")]
    public bool useBoundaries = true;
    public float minX = -15f;
    public float maxX = 15f;

    [Header("Energy Settings")]
    public bool useEnergySystem = true;
    public float runEnergyCost = 3f;
    public float minRunEnergy = 25f;

    [Header("Jumpscare Settings")]
    public float jumpscareTime = 0.3f;
    private bool wasHitByStalker = false;
    public AudioSource jumpscareAudio;
    public AudioClip jumpscare;

    private PlayerInput playerInput;
    private CapsuleCollider2D col;
    private SpriteRenderer spriteRenderer;

    // [ADD] เพิ่มตัวแปร Animator
    private Animator animator;


    // ------------------------------------------------------------
    // Initialization
    // ------------------------------------------------------------
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();

        // [ADD] ดึง Component Animator มาเก็บไว้
        animator = GetComponent<Animator>();

        if (hideImage != null)
            hideImage.gameObject.SetActive(false);

        if (warningImage != null)
        {
            Color c = warningImage.color;
            c.a = 0f;
            warningImage.color = c;
        }
    }


    // ------------------------------------------------------------
    // Update Loop
    // ------------------------------------------------------------
    void Update()
    {
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        float x = input.x;
        float y = input.y;

        float inputMagnitude = Mathf.Abs(x);
        float currentEnergy = GameManager.instance.energy;

        // ---------------- Movement & Run Logic ----------------
        // ---------------- Movement & Run Logic ----------------
        if (inputMagnitude > 0.1f && inputMagnitude <= 0.6f)
        {
            // เดินปกติ
            currentSpeed = walkSpeed;
            GameManager.instance.isRunning = false;
        }
        else if (inputMagnitude > 0.6f)
        {
            bool canRun = currentEnergy >= minRunEnergy;  // minRunEnergy = 25

            if (canRun)
            {
                // วิ่งได้
                currentSpeed = runSpeed;
                GameManager.instance.isRunning = true;
            }
            else
            {
                // ❗ ห้ามวิ่ง (พลังไม่พอ)
                currentSpeed = walkSpeed;
                GameManager.instance.isRunning = false;
            }
        }
        else
        {
            // ❗ กรณีผู้เล่นไม่กดปุ่ม → ความเร็ว = 0 และต้องไม่ถือว่า “กำลังวิ่ง”
            currentSpeed = 0f;
            GameManager.instance.isRunning = false;
        }



        // [ADD] -------------- Animation Logic ---------------
        if (animator != null)
        {
            // ส่งค่าความเร็วปัจจุบันไปที่ Animator
            // (ต้องไปสร้าง Parameter แบบ Float ชื่อ "Speed" ใน Animator Controller ด้วย)
            animator.SetFloat("Speed", currentSpeed);

            // Optional: ยังคงเก็บ IsMoving ไว้เผื่อใช้เปลี่ยน state แบบปกติ
            bool isMoving = inputMagnitude > 0.01f;
            animator.SetBool("IsMoving", isMoving);
        }

        // [ADD] -------------- Flip Sprite ---------------
        // หันหน้าซ้ายขวาตามค่า x
        if (x > 0)
        {
            spriteRenderer.flipX = false; // หันขวา (สมมติภาพต้นฉบับหันขวา)
        }
        else if (x < 0)
        {
            spriteRenderer.flipX = true;  // หันซ้าย
        }
        // ----------------------------------------------------


        // ---------------- Door Enter ----------------
        if (playerIsNearDoor && y > 0.8f)
        {
            GameManager.instance.lastPlayerPosition = transform.position;
            doorClick.OpenDoor();
        }

        // ---------------- Hide Enter / Exit ----------------
        if (playerIsNearHide && y > 0.8f && !isHidden)
            StartHiding();
        else if (isHidden && y < -0.8f)
            StopHiding();

        // ---------------- Stairs Teleport ----------------
        if (playerIsNearStairs && y > 0.8f)
        {
            transform.position = targetPosition.position;
            playerIsNearStairs = false;
        }

        // ---------------- Movement ----------------
        if (isHidden || stopX)
            x = 0f;

        Vector3 move = new Vector3(x, 0, 0);
        Vector3 newPosition = transform.position + move * currentSpeed * Time.deltaTime;

        if (useBoundaries)
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        transform.position = newPosition;
        GameManager.instance.isMoving = currentSpeed > 0;

        // ---------------- Energy Use ----------------
        if (useEnergySystem && GameManager.instance.isRunning && currentEnergy > 0)
            GameManager.instance.UseEnergy(runEnergyCost * Time.deltaTime);

        // ---------------- Footstep Sound ----------------
        // อ่าน input เดิน
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        bool isMovingCheck = Mathf.Abs(moveInput.x) > 0.1f; // เปลี่ยนชื่อตัวแปรกันชนกับข้างบน
        bool isRunning = GameManager.instance.isRunning;

        // --------- FOOTSTEP SYSTEM ----------
        if (isMovingCheck)
        {
            float interval = isRunning ? runInterval : walkInterval;

            // เช็ค clip ที่ควรเล่นตอนนี้
            AudioClip targetClip = isRunning ? runClip : walkClip;

            // ถ้า clip ไม่ตรงกับที่ควรเล่น → เปลี่ยน clip และเริ่มเล่นใหม่
            if (footstepSource.clip != targetClip)
            {
                footstepSource.clip = targetClip;
                footstepSource.Stop();
                footstepTimer = 0f;  // รีเซ็ตเพื่อให้เสียงเริ่มใหม่พอดีจังหวะ
            }

            footstepTimer -= Time.deltaTime;

            // ถึงเวลาเล่นเสียงใหม่
            if (footstepTimer <= 0f)
            {
                // random pitch ให้เสียงไม่ซ้ำ
                footstepSource.pitch = Random.Range(0.95f, 1.05f);

                // เล่นเสียงทีละครั้ง (ไม่ซ้อน)
                footstepSource.Play();

                footstepTimer = interval;
            }
        }
        else
        {
            // ถ้าไม่เดิน ให้หยุดเสียงทันที
            if (footstepSource.isPlaying)
                footstepSource.Stop();

            footstepTimer = 0f;
        }
    }


    // ------------------------------------------------------------
    // Hiding System
    // ------------------------------------------------------------
    void StartHiding()
    {
        hidingSource.PlayOneShot(openDoor);
        hidingSource.clip = hidingClip;
        hidingSource.Play();
        isHidden = true;
        spriteRenderer.enabled = false;
        col.enabled = false;
        hideImage.gameObject.SetActive(true);

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        damageCoroutine = StartCoroutine(HideDamageRoutine());
    }

    void StopHiding()
    {
        hidingSource.PlayOneShot(openDoor);
        if (hidingSource.isPlaying)
        { 
            hidingSource.Stop(); 
        }
        isHidden = false;
        spriteRenderer.enabled = true;
        col.enabled = true;
        hideImage.gameObject.SetActive(false);

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        StopAllCoroutines(); // stop fade routines

        if (warningImage != null)
        {
            Color c = warningImage.color;
            c.a = 0f;
            warningImage.color = c;
        }
    }

    IEnumerator HideDamageRoutine()
    {
        float half = safeHideTime / 2f;

        yield return new WaitForSeconds(half);

        if (isHidden && warningImage != null)
            StartCoroutine(FadeWarning(1f));

        yield return new WaitForSeconds(half);

        if (isHidden)
        {
            GameManager.instance.TakeDamage((int)damageRate);

            if (warningImage != null)
                StartCoroutine(FadeWarning(0f));

            StopHiding();
        }
    }

    IEnumerator FadeWarning(float targetAlpha)
    {
        float speed = targetAlpha > 0 ? fadeInSpeed : fadeOutSpeed;

        Color c = warningImage.color;

        while (!Mathf.Approximately(c.a, targetAlpha))
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * speed);
            warningImage.color = c;
            yield return null;
        }
    }


    // ------------------------------------------------------------
    // Trigger Interactions
    // ------------------------------------------------------------
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            doorClick = other.GetComponent<DoorClick>();
            playerIsNearDoor = true;
        }
        else if (other.CompareTag("HideSpot"))
        {
            playerIsNearHide = true;
        }
        else if (other.CompareTag("Stalker"))
        {
            
          jumpscareAudio.PlayOneShot(jumpscare);
            wasHitByStalker = true;
          GameManager.instance.TakeDamage(100);
          StartCoroutine(DoJumpscare());
            
        }
        else if (other.CompareTag("Win"))
        {
            winPanel.SetActive(true);
        }
        else if (other.CompareTag("stairs"))
        {
            playerIsNearStairs = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            doorClick = null;
            playerIsNearDoor = false;
        }
        else if (other.CompareTag("HideSpot"))
        {
            playerIsNearHide = false;
        }
        else if (other.CompareTag("stairs"))
        {
            playerIsNearStairs = false;
        }
    }


    // ------------------------------------------------------------
    // Jumpscare System
    // ------------------------------------------------------------
    IEnumerator DoJumpscare()
    {
        if (jumpscareImage != null)
            jumpscareImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(jumpscareTime);

        if (jumpscareImage != null)
            jumpscareImage.gameObject.SetActive(false);
    }
}