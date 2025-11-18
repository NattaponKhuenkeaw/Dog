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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Transform player = other.transform;
            float dir = player.localScale.x > 0 ? -1 : 1;
            Vector3 spawnPos = player.position + new Vector3(dir * distanceBehind, 0, 0);

            Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
            Debug.Log("Spawn behind player at: " + spawnPos);

            // --- ส่วนที่แก้ไข ---

            // 1. สั่งกระพริบไฟดวงเดิมที่มีอยู่ใน Array
            foreach (var light in lightsToBlink)
            {
                if (light != null) // ตรวจสอบว่า light ไม่ได้เป็นค่าว่าง
                {
                    StartCoroutine(BlinkTempLight(light));
                }
            }

            // 2. ปิด Collider แทนการทำลาย Object 
            //    เพื่อไม่ให้ Trigger ทำงานซ้ำ แต่ยังรัน Coroutine ต่อได้
            GetComponent<Collider2D>().enabled = false;

            // Destroy(gameObject); // <--- ปัญหาอยู่ที่บรรทัดนี้ครับ
        }
    }

    private IEnumerator BlinkTempLight(Light2D tempLight)
    {
        Debug.Log("Blink start: " + tempLight.name);

        float originalIntensity = tempLight.intensity;

        for (int i = 0; i < blinkCount; i++)
        {
            tempLight.intensity = 0;
            Debug.Log("OFF");
            yield return new WaitForSeconds(blinkDuration);

            tempLight.intensity = originalIntensity;
            Debug.Log("ON");
            yield return new WaitForSeconds(blinkDuration);
        }

        tempLight.intensity = originalIntensity;
        Debug.Log("END");
    }
}