using UnityEngine;

public class Stalker : MonoBehaviour
{
    [Header("Definition")]
    [SerializeField] private EnemyDefinition definition;

    public float speed = 8f;
    public float lifeAfterPass = 1f;

    private Transform player;
    private bool hasPassed;

    private void Start()
    {
        ApplyDefinition();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found. Make sure the player GameObject uses the Player tag.");
        }
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        if (!hasPassed)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.position) < 0.3f)
            {
                hasPassed = true;
            }
        }
        else
        {
            transform.Translate(transform.right * speed * Time.deltaTime);

            lifeAfterPass -= Time.deltaTime;
            if (lifeAfterPass <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ApplyDefinition()
    {
        if (definition == null)
        {
            return;
        }

        speed = definition.BaseSpeed;
        lifeAfterPass = definition.DespawnDelay;
    }
}
