using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHP : MonoBehaviour
{
    public int maxHP = 3;
    private int currentHP;
    private float spawnTime;

    public Renderer targetRenderer;
    public Color hitColor = Color.red;
    public float hitFlashDuration = 0.2f;

    private Color originalColor;
    private Coroutine flashRoutine;

    [Header("HP UI")]
    public GameObject hpBarPrefab;
    private Image hpFill;
    private Transform hpBarInstance;

    void Start()
    {
        currentHP = maxHP;
        spawnTime = Time.time;

        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();

        if (targetRenderer != null)
            originalColor = targetRenderer.material.color;

        if (hpBarPrefab != null)
        {
            hpBarInstance = Instantiate(hpBarPrefab, transform).transform;
            float scaleFactor = 1f / transform.lossyScale.x;
            hpBarInstance.localScale = Vector3.one * scaleFactor;
            hpBarInstance.localPosition = new Vector3(0f, -scaleFactor, 0f);
            

            hpFill = hpBarInstance.Find("HPFill").GetComponent<Image>();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // ✅ スコア加算：ダメージ
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddDamageScore(damage);
            ScoreManager.Instance.ShowScorePopup(damage * 10, transform.position + Vector3.up * 1.5f);
        }


        // ヒット演出
        if (targetRenderer != null)
        {
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(FlashColor());
        }

        // HPバー更新
        if (hpFill != null)
        {
            hpFill.fillAmount = Mathf.Clamp01((float)currentHP / maxHP);
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashColor()
    {
        targetRenderer.material.color = hitColor;
        yield return new WaitForSeconds(hitFlashDuration);
        targetRenderer.material.color = originalColor;
    }

    private void Die()
    {
        // ✅ スコア加算：撃破
        if (ScoreManager.Instance != null)
        {
            bool isMoving = GetComponent<EnemyMovement>() != null;
            float killDuration = Time.time - spawnTime;

            ScoreManager.Instance.RegisterKill(isMoving, killDuration);
            ScoreManager.Instance.ShowScorePopup(300, transform.position + Vector3.up * 2f);

        }

        Destroy(gameObject);
    }
}
