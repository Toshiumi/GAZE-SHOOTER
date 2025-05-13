using UnityEngine;
using UnityEngine.UI;
using ViveSR.anipal.Eye;

public class GazeUIInteractor : MonoBehaviour
{
    public float blinkThreshold = 0.05f; // まばたき継続時間
    private GameObject lastSelected = null;

    void Update()
    {
        var gaze = EyeGazeRay.Instance;
        var blink = BlinkShooter.Instance;

        if (gaze == null || blink == null || !gaze.gotGaze) return;

        // Raycastがヒットしているオブジェクトを取得
        GameObject hitObj = gaze.gazeHit.collider?.gameObject;
        if (hitObj == null) return;

        // Buttonコンポーネントを持っているか？
        Button btn = hitObj.GetComponent<Button>();
        if (btn == null) return;

        // 前フレームと違うボタンを狙っている場合のみ、ハイライト
        if (hitObj != lastSelected)
        {
            lastSelected = hitObj;
            btn.Select(); // ボタンをハイライト
        }

        if (btn != null)
        {
            Debug.Log($"Gaze on button: {btn.name}");
        }

        // まばたきでクリック
        if (blink.IsBothEyesClosed && blink.BlinkDuration > blinkThreshold)
        {
            btn.onClick.Invoke();
        }
    }
}
