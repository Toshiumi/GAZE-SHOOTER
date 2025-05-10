using UnityEngine;
using ViveSR.anipal.Eye;

public class GazeTargetMarker : MonoBehaviour
{
    public Transform markerObject; // �Ə��}�[�J�[�i��F3D�A�C�R���j
    public float followSpeed = 10f;

    void Update()
    {
        if (EyeGazeRay.Instance == null || markerObject == null) return;

        if (EyeGazeRay.Instance.IsHittingEnemy)
        {
            // �q�b�g�|�C���g�ւȂ߂炩�Ɉړ��E�\��
            Vector3 targetPos = EyeGazeRay.Instance.gazeHit.point;
            markerObject.position = Vector3.Lerp(markerObject.position, targetPos, Time.deltaTime * followSpeed);
            markerObject.LookAt(Camera.main.transform);
            markerObject.gameObject.SetActive(true);
        }
        else
        {
            // ��\��
            markerObject.gameObject.SetActive(false);
        }
    }
}
