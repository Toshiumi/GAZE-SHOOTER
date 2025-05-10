using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 5f; // ïbå„Ç…é©ìÆè¡ñ≈

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyHP enemy = collision.collider.GetComponent<EnemyHP>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
