using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FL_Manager : MonoBehaviour
{
    [Header("Flicker Settings")]
    public bool enableFlicker = true;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.2f;
    public float flickerSpeed = 0.1f;

    [Header("Damage Settings")]
    public float flashlightRange = 5f;
    public float timeToDamageEnemy = 1f;
    public LayerMask enemyLayer;

    private Light2D light2D;
    private float flickerTimer;
    private Dictionary<Enemy, float> enemiesHit = new Dictionary<Enemy, float>();

    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        // ปิดทุกระบบถ้าไฟฉายไม่เปิด
        if (GameManager.instance == null || !GameManager.instance.flashlightOn)
        {
            enemiesHit.Clear();
            return;
        }

        // 🔹 ระบบไฟกะพริบ
        if (enableFlicker)
        {
            flickerTimer += Time.deltaTime;
            if (flickerTimer >= flickerSpeed)
            {
                light2D.intensity = Random.Range(minIntensity, maxIntensity);
                flickerTimer = 0f;
            }
        }

        //  ระบบทำดาเมจศัตรู
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, flashlightRange, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null && !enemiesHit.ContainsKey(enemy))
                enemiesHit.Add(enemy, 0f);
        }

        // ตรวจว่าศัตรูอยู่ในรัศมีนานพอหรือยัง
        List<Enemy> toRemove = new List<Enemy>();
        foreach (var kvp in enemiesHit.ToList())
        {
            Enemy enemy = kvp.Key;
            if (enemy == null)
            {
                toRemove.Add(enemy);
                continue;
            }

            enemiesHit[enemy] += Time.deltaTime;
            if (enemiesHit[enemy] >= timeToDamageEnemy)
            {
                enemy.DamagePlayer();
                toRemove.Add(enemy);
            }
        }

        foreach (Enemy e in toRemove)
            enemiesHit.Remove(e);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, flashlightRange);
    }
}
