using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlashlightDamage : MonoBehaviour
{
    public float flashlightRange = 5f;
    public float timeToDamagePlayer = 1f;
    public LayerMask enemyLayer;

    private Dictionary<Enemy, float> enemiesHit = new Dictionary<Enemy, float>();

    void Update()
    {
        if (GameManager.instance == null || !GameManager.instance.flashlightOn || GameManager.instance.flashlight2D == null)
        {
            enemiesHit.Clear();
            return;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, flashlightRange, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null && !enemiesHit.ContainsKey(enemy))
                enemiesHit.Add(enemy, 0f);
        }

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
            if (enemiesHit[enemy] >= timeToDamagePlayer)
            {
                enemy.DamagePlayer();
                toRemove.Add(enemy);
            }
        }

        foreach (Enemy e in toRemove)
            enemiesHit.Remove(e);
    }

    public void ClearEnemies()
    {
        enemiesHit.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, flashlightRange);
    }
}
