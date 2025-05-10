using UnityEngine;
using ViveSR.anipal.Eye;

public class GazeTargetMarker : MonoBehaviour
{
    public Transform markerObject; // 照準マーカー（例：3Dアイコン）
    public float followSpeed = 10f;

    void Update()
    {
        if (EyeGazeRay.Instance == null || markerObject == null) return;

        if (EyeGazeRay.Instance.IsHittingEnemy)
        {
            // ヒットポイントへなめらかに移動・表示
            Vector3 targetPos = EyeGazeRay.Instance.gazeHit.point;
            markerObject.position = Vector3.Lerp(markerObject.position, targetPos, Time.deltaTime * followSpeed);
            markerObject.LookAt(Camera.main.transform);
            markerObject.gameObject.SetActive(true);
        }
        else
        {
            // 非表示
            markerObject.gameObject.SetActive(false);
        }
    }
}
