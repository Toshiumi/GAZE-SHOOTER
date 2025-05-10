using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public int maxHP = 3;
    private int currentHP;

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

        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();

        if (targetRenderer != null)
            originalColor = targetRenderer.material.color;

        // HPバーの生成と配置
        if (hpBarPrefab != null)
        {
            hpBarInstance = Instantiate(hpBarPrefab, transform).transform;
            hpBarInstance.localPosition = new Vector3(0f, -1.0f, 0f); // 足元に配置（調整可）
            hpFill = hpBarInstance.Find("HPFill").GetComponent<Image>(); // HPBarCanvas 内の "HPFill" という名前の子オブジェクトを探す

        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

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
            Debug.Log("teetetet");
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
        Destroy(gameObject);
    }
}
