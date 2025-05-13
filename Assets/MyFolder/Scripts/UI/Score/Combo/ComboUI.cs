using UnityEngine;
using TMPro;

public class ComboUI : MonoBehaviour
{
    public Transform playerHead;
    public Vector3 offset = new Vector3(0f, 0.2f, 2f); // 視界の少し上
    public TextMeshProUGUI comboText;
    public float displayDuration = 2f;

    private float timer = 0f;
    private int lastCombo = 0;

    void Start()
    {
        if (playerHead == null && Camera.main != null)
            playerHead = Camera.main.transform;
        comboText.text = "";
    }

    void Update()
    {
        if (playerHead == null || ScoreManager.Instance == null) return;

        // カメラの前方に追従
        transform.position = playerHead.position + playerHead.rotation * offset;
        transform.LookAt(playerHead);
        transform.rotation = Quaternion.LookRotation(transform.position - playerHead.position);

        int combo = ScoreManager.Instance.GetComboCount();

        if (combo >= 2 && combo != lastCombo)
        {
            ShowCombo(combo);
        }

        // 表示時間経過で非表示
        if (comboText.text != "")
        {
            timer += Time.deltaTime;
            if (timer >= displayDuration)
            {
                comboText.text = "";
                timer = 0f;
            }
        }

        lastCombo = combo;
    }

    private void ShowCombo(int combo)
    {
        comboText.text = $"COMBO x{combo}!!";
        comboText.color = Color.Lerp(Color.yellow, Color.red, combo / 10f);
        timer = 0f;
    }
}
