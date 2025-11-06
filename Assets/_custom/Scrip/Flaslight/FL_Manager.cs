using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class FL_Manager : MonoBehaviour
{
    [Header("Jumpscare Settings")]
    public Image jumpscareImage;    // รูป/วิดีโอ jumpscare ที่จะโผล่
                                    // public AudioSource jumpscareSound;   // เสียงน่ากลัว
    public float scareDuration = 1.5f;   // ระยะเวลาที่แสดง
    public float fadeSpeed = 5f;


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

        Color color = jumpscareImage.color;
        color.a = 0f;
        jumpscareImage.color = color;
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
                StartCoroutine(DoJumpscare());
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

    IEnumerator DoJumpscare()
    {
        //if (jumpscareSound != null)
        //  jumpscareSound.Play();

        // เฟดเข้าภาพ
        yield return StartCoroutine(FadeImage(1f));

        // ค้างไว้ตามเวลา scareDuration
        yield return new WaitForSeconds(scareDuration);

        // เฟดออกภาพ
        yield return StartCoroutine(FadeImage(0f));
    }
    IEnumerator FadeImage(float targetAlpha)
    {
        Color color = jumpscareImage.color;
        while (!Mathf.Approximately(color.a, targetAlpha))
        {
            color.a = Mathf.MoveTowards(color.a, targetAlpha, Time.deltaTime * fadeSpeed);
            jumpscareImage.color = color;
            yield return null;
        }
    }
}
