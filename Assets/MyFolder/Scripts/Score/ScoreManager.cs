using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("��{�X�R�A")]
    [SerializeField] private int baseKillScore = 100;
    [SerializeField] private int movingEnemyBonus = 200;

    [Header("�_���[�W�E�����X�R�A")]
    [SerializeField] private int scorePerDamage = 10;
    [SerializeField] private int bulletHitScore = 50;

    [Header("���ԃ{�[�i�X")]
    [SerializeField] private int killTimeBonus_1s = 200;
    [SerializeField] private int killTimeBonus_2s = 100;

    [Header("�R���{�ݒ�")]
    [SerializeField] private float comboWindow = 3f;
    [SerializeField] private float comboMultiplierStep = 0.2f;

    [Header("�|�b�v�A�b�v�\��")]
    [SerializeField] private GameObject scorePopupPrefab;
    [SerializeField] private Canvas worldCanvas;


    private int score = 0;
    private int comboCount = 0;
    private float lastKillTime = 0f;

    void Awake()
    {
        Debug.Log("ScoreManager Awake �N��");

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        Debug.Log($"SCORE: {score}");
    }


    public void AddScore(int baseScore)
    {
        float comboMultiplier = 1f + (comboCount * comboMultiplierStep);
        int finalScore = Mathf.RoundToInt(baseScore * comboMultiplier);
        score += finalScore;

        Debug.Log($"Score +{finalScore}�i�R���{ {comboCount}�j �� ���X�R�A: {score}");
    }

    public void AddDamageScore(int damage)
    {
        AddScore(damage * scorePerDamage);
    }

    public void AddBulletHitScore()
    {
        AddScore(bulletHitScore);
    }

    public void RegisterKill(bool isMovingEnemy, float killDuration)
    {
        int totalScore = baseKillScore;
        if (isMovingEnemy) totalScore += movingEnemyBonus;

        // ���Ԃɂ��{�[�i�X
        if (killDuration < 1f) totalScore += killTimeBonus_1s;
        else if (killDuration < 2f) totalScore += killTimeBonus_2s;

        // �R���{�`�F�b�N
        float now = Time.time;
        if (now - lastKillTime <= comboWindow)
        {
            comboCount++;
        }
        else
        {
            comboCount = 1;
        }

        lastKillTime = now;
        AddScore(totalScore);
    }


    public void ShowScorePopup(int value, Vector3 worldPosition)
    {
        if (scorePopupPrefab != null && worldCanvas != null)
        {
            GameObject popup = Instantiate(scorePopupPrefab, worldCanvas.transform);
            popup.transform.position = worldPosition;

            var popupScript = popup.GetComponent<ScorePopup>();
            popupScript?.Setup(value);
        }
    }


    public int GetScore() => score;
    public int GetComboCount() => comboCount;
}
