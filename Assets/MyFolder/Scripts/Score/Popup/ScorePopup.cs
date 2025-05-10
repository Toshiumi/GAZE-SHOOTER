using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    public float floatSpeed = 0.5f;
    public float duration = 1.0f;
    public TextMeshProUGUI popupText;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(int score)
    {
        popupText.text = $"+{score}";
        popupText.color = Color.yellow;
    }
}
