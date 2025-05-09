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
        // TODO: �A�j���[�V�����E�����Ȃǂ̉��o�������ɒǉ��\
        Destroy(gameObject);
    }
}
