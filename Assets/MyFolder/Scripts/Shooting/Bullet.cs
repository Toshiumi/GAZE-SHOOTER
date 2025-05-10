using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 5f;
    public AudioClip hitSound;             // Inspectorで設定
    public AudioSource audioSourcePrefab;  // 使い捨てAudioSourceプレハブ

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

            // 🔊 ヒット音再生
            if (hitSound != null && audioSourcePrefab != null)
            {
                AudioSource audio = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
                audio.clip = hitSound;
                audio.Play();
                Destroy(audio.gameObject, hitSound.length);
            }

            //スコア加算
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddBulletHitScore();

        }

        Destroy(gameObject);
    }
}
