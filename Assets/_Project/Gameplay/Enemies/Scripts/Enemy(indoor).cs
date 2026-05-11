using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Definition")]
    [SerializeField] private EnemyDefinition definition;

    public int damageToPlayer = 10;

    [Header("Cry Sound")]
    public AudioClip cryClip;

    private AudioSource audioSource;

    private void Start()
    {
        ApplyDefinition();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;

        if (cryClip != null)
        {
            audioSource.PlayOneShot(cryClip);
        }
    }

    public void DamagePlayer()
    {
        Services.Health?.TakeDamage(damageToPlayer);
        Debug.Log($"{name} damaged player for {damageToPlayer}!");
        Destroy(gameObject);
    }

    private void ApplyDefinition()
    {
        if (definition == null)
        {
            return;
        }

        damageToPlayer = definition.BaseDamage;
        if (definition.WarningAudio != null)
        {
            cryClip = definition.WarningAudio;
        }
    }
}
