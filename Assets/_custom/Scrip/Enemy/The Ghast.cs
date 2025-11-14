using UnityEngine;

public class TheGhast : MonoBehaviour
{
    [Header("Timing")]
    public float greenDuration = 3f;
    public float redDuration = 2f;

    [Header("Damage")]
    public int damageOnMove = 20;       // ❗ โดนดาเมจครั้งเดียว
    public float detectionRange = 6f;

    private Transform player;
    private bool isRedLight = false;
    private float timer = 0;

    private bool hasHitThisRed = false; // กันตีซ้ำ

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

        timer = greenDuration;
    }

    void Update()
    {
        if (player == null) return;

        LightCycle();
        DetectMovementOnce();
    }

    void LightCycle()
    {
        timer -= Time.deltaTime;

        if (!isRedLight)
        {
            // GREEN
            if (timer <= 0)
            {
                isRedLight = true;
                hasHitThisRed = false; // รีเซ็ตตีใหม่รอบนี้ได้
                timer = redDuration;
                Debug.Log("🔴 RED LIGHT — ห้ามขยับ!");
            }
        }
        else
        {
            // RED
            if (timer <= 0)
            {
                isRedLight = false;
                timer = greenDuration;
                Debug.Log("🟢 GREEN LIGHT — เดินได้!");
            }
        }
    }

    void DetectMovementOnce()
    {
        if (!isRedLight) return;

        float dist = Vector2.Distance(player.position, transform.position);
        if (dist > detectionRange) return;

        if (GameManager.instance != null && GameManager.instance.isMoving)
        {
            if (!hasHitThisRed)
            {
                GameManager.instance.TakeDamage(damageOnMove);
                hasHitThisRed = true; // ตีครั้งเดียวในรอบนี้
                Debug.Log("💥 ผู้เล่นขยับตอน RED → โดนตีครั้งเดียว!");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isRedLight ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}