 using UnityEngine;

public class Stalker : MonoBehaviour
{
    public float speed = 8f;         // ความเร็วผี
    public float lifeAfterPass = 1f; // ผ่่านไปแล้วกี่วิให้หาย
    private Transform player;

    private bool hasPassed = false;

    void Start()
    {
        // หา player ในฉาก
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogError("❌ ไม่เจอ Player! ต้องตั้ง Tag เป็น 'Player'");
    }

    void Update()
    {
        if (player == null) return;

        // ถ้าผียังไม่ผ่าน → วิ่งหาผู้เล่น
        if (!hasPassed)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.Translate(dir * speed * Time.deltaTime);

            // เช็คว่าผีผ่านผู้เล่นไปแล้ว
            if (Vector2.Distance(transform.position, player.position) < 0.3f)
            {
                hasPassed = true;
            }
        }
        else
        {
            // ผีผ่านไปแล้วให้วิ่งต่อไปข้างหน้า
            transform.Translate(transform.right * speed * Time.deltaTime);

            // นับเวลาหลังผ่าน
            lifeAfterPass -= Time.deltaTime;
            if (lifeAfterPass <= 0)
            {
                Destroy(gameObject); // หายไป
            }
        }
    }
}