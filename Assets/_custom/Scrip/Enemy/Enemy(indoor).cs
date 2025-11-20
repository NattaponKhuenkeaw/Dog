using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageToPlayer = 10;

    [Header("Cry Sound")]
    public AudioClip cryClip; // ลากแค่คลิปใน prefab ก็พอ

    private AudioSource audioSource;

    void Start()
    {
        // สร้าง AudioSource ขึ้นมาในตัว Enemy ตอน Spawn
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 1f; // ให้เป็นเสียง 3D
        audioSource.playOnAwake = false;

        if (cryClip != null)
        {
            audioSource.PlayOneShot(cryClip);
        }
    }

    public void DamagePlayer()
    {
        if (GameManager.instance != null)
            GameManager.instance.TakeDamage(damageToPlayer);

        Debug.Log($"{name} damaged player for {damageToPlayer}!");
        Destroy(gameObject);
    }
}
