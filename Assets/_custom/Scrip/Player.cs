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

    [Header("Jumpscare Settings")]
    public float jumpscareTime = 0.3f;
    private bool wasHitByStalker = false;

    private PlayerInput playerInput;
    private CapsuleCollider2D col;
    private SpriteRenderer spriteRenderer;


    // ------------------------------------------------------------
    // Initialization
    // ------------------------------------------------------------
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();

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
        if (inputMagnitude > 0.1f && inputMagnitude <= 0.6f)
        {
            currentSpeed = walkSpeed;
            GameManager.instance.isRunning = false;
        }
        else if (inputMagnitude > 0.6f)
        {
            currentSpeed = currentEnergy > 0 ? runSpeed : walkSpeed;
            GameManager.instance.isRunning = (currentSpeed == runSpeed);
        }
        else
        {
            currentSpeed = 0f;
            GameManager.instance.isRunning = false;
        }

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

        Vector3 move = new(x, 0, 0);
        Vector3 newPosition = transform.position + move * currentSpeed * Time.deltaTime;

        if (useBoundaries)
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        transform.position = newPosition;
        GameManager.instance.isMoving = currentSpeed > 0;

        // ---------------- Energy Use ----------------
        if (useEnergySystem && GameManager.instance.isRunning && currentEnergy > 0)
            GameManager.instance.UseEnergy(runEnergyCost * Time.deltaTime);
    }


    // ------------------------------------------------------------
    // Hiding System
    // ------------------------------------------------------------
    void StartHiding()
    {
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
            if (!wasHitByStalker)
            {
                wasHitByStalker = true;
                GameManager.instance.TakeDamage(25);
                StartCoroutine(DoJumpscare());
            }
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
