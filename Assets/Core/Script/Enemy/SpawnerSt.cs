using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class SpawnerSt : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float distanceBehind = 1f;
    public Light2D[] lightsToBlink;
    public float blinkDuration = 0.2f;
    public int blinkCount = 3;

    [Header("Sound Effect")]
    public AudioSource spawnSound;   // ★ เพิ่มตัวแปรเสียง

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Transform player = other.transform;
            float dir = player.localScale.x > 0 ? -1 : 1;
            Vector3 spawnPos = player.position + new Vector3(dir * distanceBehind, 0, 0);

            Instantiate(objectToSpawn, spawnPos, Quaternion.identity);

            // 🔊 เล่นเสียงตรงนี้
            if (spawnSound != null)
                spawnSound.Play();

            // กระพริบไฟ
            foreach (var light in lightsToBlink)
            {
                if (light != null)
                    StartCoroutine(BlinkTempLight(light));
            }

            // ปิด Collider เพื่อไม่ให้ทำงานซ้ำ
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private IEnumerator BlinkTempLight(Light2D tempLight)
    {
        float originalIntensity = tempLight.intensity;

        for (int i = 0; i < blinkCount; i++)
        {
            tempLight.intensity = 0;
            yield return new WaitForSeconds(blinkDuration);

            tempLight.intensity = originalIntensity;
            yield return new WaitForSeconds(blinkDuration);
        }

        tempLight.intensity = originalIntensity;
    }
}
