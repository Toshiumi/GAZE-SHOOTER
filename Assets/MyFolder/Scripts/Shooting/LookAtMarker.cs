using UnityEngine;

public class LookAtMarker : MonoBehaviour
{
    public float appearSpeed = 8f;
    public float disappearSpeed = 12f;
    public float pulseAmplitude = 0.1f;
    public float pulseFrequency = 2f;
    public float maxScale = 0.05f;

    private bool isVisible = false;
    private Vector3 baseScale;
    private float scaleLerp;

    void Start()
    {
        baseScale = Vector3.one * maxScale;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        // ��ʂ̐��ʂ���������i�C�Ӂj
        transform.LookAt(Camera.main.transform);

        // Pulse �G�t�F�N�g�i�g��k���j
        float pulse = Mathf.Sin(Time.time * pulseFrequency) * pulseAmplitude;
        Vector3 targetScale = isVisible ? baseScale * (1f + pulse) : Vector3.zero;

        scaleLerp = isVisible ? appearSpeed : disappearSpeed;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleLerp);
    }

    public void Show() => isVisible = true;
    public void Hide() => isVisible = false;
}
