using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;

    public float centerDistance = 5f;  // �v���C���[�̐��ʉ�m
    public float moveRange = 7f;       // ���E�ړ��͈́i�}�l�j
    public float moveSpeed = 2f;       // ���E�ړ����x

    public float swayAmplitude = 0.5f;
    public float swayFrequency = 2f;
    public Vector3 swayAxis = Vector3.up;

    private float swayTime = 0f;
    private float direction = 1f; // ���E�̉�������i1 or -1�j
    private Vector3 centerPos;
    private float currentOffset = 0f;

    void Start()
    {
        if (player == null)
            player = Camera.main.transform;

        // ����5m���ړ��̒�����_
        centerPos = transform.position;

        // ���������̏����I�t�Z�b�g�i�����_���ł�OK�j
        currentOffset = Random.Range(-moveRange, moveRange);
    }

    void Update()
    {
        // ���E�̉����ړ��icurrentOffset�𓮂����j
        currentOffset += direction * moveSpeed * Time.deltaTime;

        // �͈͊O�ɏo���甽�]
        if (Mathf.Abs(currentOffset) > moveRange)
        {
            direction *= -1f;
            currentOffset = Mathf.Clamp(currentOffset, -moveRange, moveRange);
        }

        

        // �㉺�̂����
        swayTime += Time.deltaTime;
        Vector3 swayOffset = swayAxis.normalized * Mathf.Sin(swayTime * swayFrequency) * swayAmplitude;

        // ���[���h�E�����ō��E�ɓ����iY�h�ꍞ�݁j
        transform.position = centerPos + Vector3.right * currentOffset + swayOffset;
    }
}
