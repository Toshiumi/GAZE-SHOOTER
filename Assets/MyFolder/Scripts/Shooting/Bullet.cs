using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 5f;
    public AudioClip hitSound;
    public AudioSource audioSourcePrefab;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hitObj = collision.collider.gameObject;

        // ボタンに当たったかチェック
        Button button = hitObj.GetComponent<Button>();
        if (button != null)
        {

            Debug.Log("Bullet hit UI Button: " + button.name);
            button.onClick.Invoke();
            

            if (hitSound != null && audioSourcePrefab != null)
            {
                AudioSource audio = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
                audio.clip = hitSound;
                audio.Play();
                Destroy(audio.gameObject, hitSound.length);
            }

            Destroy(gameObject); // 弾を消す
            return;
        }

        // 敵に当たった場合
        EnemyHP enemy = hitObj.GetComponent<EnemyHP>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            if (hitSound != null && audioSourcePrefab != null)
            {
                AudioSource audio = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
                audio.clip = hitSound;
                audio.Play();
                Destroy(audio.gameObject, hitSound.length);
            }

            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddBulletHitScore();
        }

        Destroy(gameObject);
    }
}
