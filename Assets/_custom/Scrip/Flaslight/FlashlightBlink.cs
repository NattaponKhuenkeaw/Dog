using UnityEngine;
using UnityEngine.Rendering.Universal; // ต้องใช้ถ้าเป็น Light2D

public class FlashlightFlicker : MonoBehaviour
{
    private Light2D light2D;

    [Header("Flicker Settings")]
    public float minIntensity = 0.5f; // ค่าความสว่างต่ำสุด
    public float maxIntensity = 1.2f; // ค่าความสว่างสูงสุด
    public float flickerSpeed = 0.1f; // ความเร็วการสุ่ม

    private float timer;

    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= flickerSpeed)
        {
            light2D.intensity = Random.Range(minIntensity, maxIntensity);
            timer = 0f;
        }
    }
}
