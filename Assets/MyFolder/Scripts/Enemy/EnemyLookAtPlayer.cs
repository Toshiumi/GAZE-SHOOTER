using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{

    void Update()
    {
        // ��ʂ̐��ʂ���������i�C�Ӂj
        transform.LookAt(Camera.main.transform);
    }
}
