using ViveSR.anipal.Eye;
using UnityEngine;

public class BlinkShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public Transform cameraOrigin;

    private bool wasEyeOpenLastFrame = true;

    private EyeData_v2 eyeData = new EyeData_v2();

    void Update()
    {
        // EyeData ‚ðŽæ“¾
        if (SRanipal_Eye_API.GetEyeData_v2(ref eyeData) == ViveSR.Error.WORK)
        {
            float leftOpenness = eyeData.verbose_data.left.eye_openness;
            float rightOpenness = eyeData.verbose_data.right.eye_openness;

            bool eyesClosed = leftOpenness < 0.3f && rightOpenness < 0.3f;

            if (wasEyeOpenLastFrame && eyesClosed)
            {
                FireBullet();
            }

            wasEyeOpenLastFrame = !eyesClosed;
        }
    }

    void FireBullet()
    {
        

        if (bulletPrefab == null || cameraOrigin == null || EyeGazeRay.Instance == null) return;

        Vector3 gazeDir = EyeGazeRay.Instance.CachedGazeDirection;

        GameObject bullet = Instantiate(bulletPrefab, cameraOrigin.position, Quaternion.LookRotation(gazeDir));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = gazeDir * bulletSpeed;
    }
}
