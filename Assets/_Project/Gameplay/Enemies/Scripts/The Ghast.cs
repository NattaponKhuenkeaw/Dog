using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TheGhast : MonoBehaviour
{
    [Header("Definition")]
    [SerializeField] private EnemyDefinition definition;

    [Header("Timing")]
    public float greenDuration = 3f;
    public float redDuration = 2f;

    [Header("Damage")]
    public int damageOnMove = 20;
    public float detectionRange = 6f;

    [Header("Light")]
    public Light2D redLight;
    public Color redLightColor = Color.red;
    public float redLightIntensity = 2f;

    private Transform player;
    private bool isRedLight;
    private float timer;
    private bool hasHitThisRed;

    private void Start()
    {
        ApplyDefinition();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        timer = greenDuration;
        if (redLight != null)
        {
            redLight.enabled = false;
        }
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        LightCycle();
        DetectMovementOnce();
    }

    private void LightCycle()
    {
        timer -= Time.deltaTime;

        if (!isRedLight)
        {
            if (timer <= 0f)
            {
                isRedLight = true;
                hasHitThisRed = false;
                timer = redDuration;

                if (redLight != null)
                {
                    redLight.enabled = true;
                    redLight.color = redLightColor;
                    redLight.intensity = redLightIntensity;
                }
            }
        }
        else if (timer <= 0f)
        {
            isRedLight = false;
            timer = greenDuration;

            if (redLight != null)
            {
                redLight.enabled = false;
            }
        }
    }

    private void DetectMovementOnce()
    {
        if (!isRedLight)
        {
            return;
        }

        float distance = Vector2.Distance(player.position, transform.position);
        if (distance > detectionRange)
        {
            return;
        }

        if (GameManager.instance != null && GameManager.instance.isMoving && !hasHitThisRed)
        {
            Services.Health?.TakeDamage(damageOnMove);
            hasHitThisRed = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isRedLight ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void ApplyDefinition()
    {
        if (definition == null)
        {
            return;
        }

        redDuration = Mathf.Max(0.1f, definition.WarningDuration);
        damageOnMove = definition.BaseDamage;
        detectionRange = definition.DetectionRange;
    }
}
