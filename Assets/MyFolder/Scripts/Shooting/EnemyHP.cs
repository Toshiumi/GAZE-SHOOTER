using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int maxHP = 3;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: アニメーション・爆発などの演出もここに追加可能
        Destroy(gameObject);
    }
}
