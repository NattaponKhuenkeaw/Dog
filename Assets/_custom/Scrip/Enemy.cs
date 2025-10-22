using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageToPlayer = 10;

    public void DamagePlayer()
    {
        if (GameManager.instance != null)
            GameManager.instance.TakeDamage(damageToPlayer);

        Debug.Log($"{name} damaged player for {damageToPlayer}!");
        Destroy(gameObject); // ทำลายตัวเอง
    }
}
