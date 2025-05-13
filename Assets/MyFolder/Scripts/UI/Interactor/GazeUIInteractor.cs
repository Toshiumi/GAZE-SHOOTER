using UnityEngine;
using UnityEngine.UI;
using ViveSR.anipal.Eye;

public class GazeUIInteractor : MonoBehaviour
{
    public float blinkThreshold = 0.05f; // �܂΂����p������
    private GameObject lastSelected = null;

    void Update()
    {
        var gaze = EyeGazeRay.Instance;
        var blink = BlinkShooter.Instance;

        if (gaze == null || blink == null || !gaze.gotGaze) return;

        // Raycast���q�b�g���Ă���I�u�W�F�N�g���擾
        GameObject hitObj = gaze.gazeHit.collider?.gameObject;
        if (hitObj == null) return;

        // Button�R���|�[�l���g�������Ă��邩�H
        Button btn = hitObj.GetComponent<Button>();
        if (btn == null) return;

        // �O�t���[���ƈႤ�{�^����_���Ă���ꍇ�̂݁A�n�C���C�g
        if (hitObj != lastSelected)
        {
            lastSelected = hitObj;
            btn.Select(); // �{�^�����n�C���C�g
        }

        if (btn != null)
        {
            Debug.Log($"Gaze on button: {btn.name}");
        }

        // �܂΂����ŃN���b�N
        if (blink.IsBothEyesClosed && blink.BlinkDuration > blinkThreshold)
        {
            btn.onClick.Invoke();
        }
    }
}
