using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void OnInteract(PlayerController player);
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float dodgeSpeed = 12f;
    public float dodgeDuration = 0.2f;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    [Header("Current Interactable")]
    public IInteractable currentInteractable;

    private float moveDir = 0f;
    private bool isDodging = false;
    private float dodgeTime;

    private PlayerControls controls;
    private float touchMoveDir = 0f;
    private bool touchAttack = false;
    private bool touchDodge = false;
    private bool touchInteract = false;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        float inputDir = controls.Player.Move.ReadValue<float>();

        if (Mathf.Abs(inputDir) < 0.01f)
        {
            if (Keyboard.current.aKey.isPressed) inputDir = -1f;
            if (Keyboard.current.dKey.isPressed) inputDir = 1f;
        }

        moveDir = touchMoveDir != 0f ? touchMoveDir : inputDir;

        if (moveDir > 0.01f) spriteRenderer.flipX = false;
        else if (moveDir < -0.01f) spriteRenderer.flipX = true;

        if (controls.Player.Attack.WasPerformedThisFrame() || touchAttack) Attack();
        if (controls.Player.Dodge.WasPerformedThisFrame() || touchDodge) Dodge();
        if (controls.Player.Interact.WasPerformedThisFrame() || touchInteract) Interact();

        touchAttack = touchDodge = touchInteract = false;
    }

    private void FixedUpdate()
    {
        float speed = isDodging ? dodgeSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(moveDir * speed, rb.linearVelocity.y);

        if (isDodging && Time.time >= dodgeTime)
            isDodging = false;
    }

    public void Attack() => Debug.Log("Attack!");
    public void Dodge()
    {
        if (!isDodging)
        {
            isDodging = true;
            dodgeTime = Time.time + dodgeDuration;
            Debug.Log("Dodge!");
        }
    }

    public void Interact()
    {
        if (currentInteractable != null)
            currentInteractable.OnInteract(this);
    }

    public void Move(float value) => touchMoveDir = value;
    public void AttackButton() => touchAttack = true;
    public void DodgeButton() => touchDodge = true;
    public void InteractButton() => touchInteract = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
            currentInteractable = interactable;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null && currentInteractable == interactable)
            currentInteractable = null;
    }
}