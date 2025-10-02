using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dodgeSpeed = 12f;
    public float dodgeDuration = 0.2f;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    private float moveDir;
    private PlayerControls controls;
    private bool isDodging = false;
    private float dodgeTime;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();

        controls.Player.MoveLeft.performed += ctx => moveDir = -1f;
        controls.Player.MoveLeft.canceled += ctx => { if (!controls.Player.MoveRight.IsPressed()) moveDir = 0f; };

        controls.Player.MoveRight.performed += ctx => moveDir = 1f;
        controls.Player.MoveRight.canceled += ctx => { if (!controls.Player.MoveLeft.IsPressed()) moveDir = 0f; };

        controls.Player.Attack.performed += OnAttack;
        controls.Player.Interact.performed += OnInteract;
        controls.Player.Dodge.performed += OnDodge;
    }

    private void OnDisable()
    {
        controls.Player.MoveLeft.performed -= ctx => moveDir = -1f;
        controls.Player.MoveLeft.canceled -= ctx => { if (!controls.Player.MoveRight.IsPressed()) moveDir = 0f; };

        controls.Player.MoveRight.performed -= ctx => moveDir = 1f;
        controls.Player.MoveRight.canceled -= ctx => { if (!controls.Player.MoveLeft.IsPressed()) moveDir = 0f; };

        controls.Player.Attack.performed -= OnAttack;
        controls.Player.Interact.performed -= OnInteract;
        controls.Player.Dodge.performed -= OnDodge;

        controls.Player.Disable();
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log("Attack!");
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log("Interact with item!");
    }

    private void OnDodge(InputAction.CallbackContext ctx)
    {
        if (!isDodging)
        {
            Debug.Log("Dodge!");
            isDodging = true;
            dodgeTime = Time.time + dodgeDuration;
        }
    }

    private void FixedUpdate()
    {
        if (isDodging)
        {
            rb.linearVelocity = new Vector2(moveDir * dodgeSpeed, rb.linearVelocity.y);

            if (Time.time >= dodgeTime)
                isDodging = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(moveDir * moveSpeed, rb.linearVelocity.y);
        }

        if (moveDir > 0.01f)
            spriteRenderer.flipX = false;
        else if (moveDir < -0.01f)
            spriteRenderer.flipX = true;
    }
}