using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{

    void Update()
    {
        // ‰æ–Ê‚Ì³–Ê‚ğŒü‚©‚¹‚éi”CˆÓj
        transform.LookAt(Camera.main.transform);
    }
}
