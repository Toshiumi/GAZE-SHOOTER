using UnityEngine;
using ViveSR.anipal.Eye;

public class GazeDamageDealer : MonoBehaviour
{
    public float damageInterval = 0.2f;
    public int damagePerTick = 1;

    [Header("Sound")]
    public AudioSource sizzleAudioSource; // ジリジリ音（loop = true）
    private bool isSizzling = false;

    private float timer = 0f;

    void Update()
    {
        var gaze = EyeGazeRay.Instance;
        bool isLooking = gaze != null && gaze.gotGaze && gaze.IsHittingEnemy;

        // サウンドの制御（視線が当たった/外れた）
        if (isLooking && !isSizzling)
        {
            StartSizzle();
        }
        else if (!isLooking && isSizzling)
        {
            StopSizzle();
        }

        // 継続ダメージ処理
        if (isLooking)
        {
            timer += Time.deltaTime;
            if (timer >= damageInterval)
            {
                timer = 0f;

                EnemyHP hp = gaze.gazeHit.collider.GetComponentInParent<EnemyHP>();
                if (hp != null)
                {
                    hp.TakeDamage(damagePerTick);
                }

                

            }
        }
        else
        {
            timer = 0f;
        }
    }

    void StartSizzle()
    {
        if (sizzleAudioSource != null)
        {
            sizzleAudioSource.loop = true;
            sizzleAudioSource.Play();
            isSizzling = true;
        }
    }

    void StopSizzle()
    {
        if (sizzleAudioSource != null)
        {
            sizzleAudioSource.Stop();
            isSizzling = false;
        }
    }
}
