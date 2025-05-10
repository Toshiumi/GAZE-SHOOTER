using UnityEngine;
using ViveSR.anipal.Eye;

public class GazeTargetMarker : MonoBehaviour
{
    public LookAtMarker marker;

    void Update()
    {
        if (EyeGazeRay.Instance == null || marker == null) return;

        var gaze = EyeGazeRay.Instance;

        if (gaze.gotGaze && gaze.IsHittingEnemy)
        {
            marker.transform.position = gaze.gazeHit.point;
            marker.Show();
        }
        else
        {
            marker.Hide();
        }
    }
}
