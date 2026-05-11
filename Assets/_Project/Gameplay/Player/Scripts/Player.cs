using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Phase 2 Settings")]
    public PlayerSettings settings;

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

    [Header("Door System")]
    public DoorClick doorClick;

    [Header("Teleport / Interaction Flags")]
    public Transform targetPosition;
    public bool playerIsNearStairs;
    public bool playerIsNearDoor;
    public bool playerIsNearHide;
    public bool stopX;

    [Header("Runtime State")]
    public bool isMoving;
    public bool isRunning;

    [Header("Hiding System")]
    public bool isHidden;
    public float damageRate = 5f;
    public float safeHideTime = 5f;
    public AudioSource hidingSource;
    public AudioClip hidingClip;
    public AudioClip openDoor;

    [Header("Warning Fade Settings")]
    public float fadeInSpeed = 2f;
    public float fadeOutSpeed = 5f;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;

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
    public AudioSource jumpscareAudio;
    public AudioClip jumpscare;

    public PlayerInput PlayerInput { get; private set; }
    public CapsuleCollider2D Collider { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerInteraction Interaction { get; private set; }
    public PlayerHiding Hiding { get; private set; }
    public PlayerAudio AudioRuntime { get; private set; }

    public float WalkSpeed => settings != null ? settings.walkSpeed : walkSpeed;
    public float RunSpeed => settings != null ? settings.runSpeed : runSpeed;
    public float WalkThreshold => settings != null ? settings.walkThreshold : 0.1f;
    public float RunThreshold => settings != null ? settings.runThreshold : 0.6f;
    public float InteractThreshold => settings != null ? settings.interactThreshold : 0.8f;
    public float RunEnergyCost => settings != null ? settings.runEnergyCost : runEnergyCost;
    public float MinRunEnergy => settings != null ? settings.minRunEnergy : minRunEnergy;
    public float WalkFootstepInterval => settings != null ? settings.walkInterval : walkInterval;
    public float RunFootstepInterval => settings != null ? settings.runInterval : runInterval;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Collider = GetComponent<CapsuleCollider2D>();
        Animator = GetComponent<Animator>();

        Movement = GetOrAddComponent<PlayerMovement>();
        Hiding = GetOrAddComponent<PlayerHiding>();
        Interaction = GetOrAddComponent<PlayerInteraction>();
        AudioRuntime = GetOrAddComponent<PlayerAudio>();

        Hiding.Initialize(this);
        Movement.Initialize(this, Hiding);
        Interaction.Initialize(this, Movement, Hiding);
        AudioRuntime.Initialize(this, Movement);
    }

    private void Start()
    {
        if (hideImage != null)
        {
            hideImage.gameObject.SetActive(false);
        }

        if (warningImage != null)
        {
            Color color = warningImage.color;
            color.a = 0f;
            warningImage.color = color;
        }

        if (jumpscareImage != null)
        {
            jumpscareImage.gameObject.SetActive(false);
        }
    }

    public void ShowWinPanel()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    public void TriggerLegacyJumpscare()
    {
        if (jumpscareAudio != null && jumpscare != null)
        {
            jumpscareAudio.PlayOneShot(jumpscare);
        }

        StartCoroutine(DoJumpscare());
    }

    private IEnumerator DoJumpscare()
    {
        if (jumpscareImage != null)
        {
            jumpscareImage.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(jumpscareTime);

        if (jumpscareImage != null)
        {
            jumpscareImage.gameObject.SetActive(false);
        }
    }

    private T GetOrAddComponent<T>() where T : Component
    {
        T component = GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }
}
