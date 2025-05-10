using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;

    public float centerDistance = 5f;  // プレイヤーの正面何m
    public float moveRange = 7f;       // 左右移動範囲（±値）
    public float moveSpeed = 2f;       // 左右移動速度

    public float swayAmplitude = 0.5f;
    public float swayFrequency = 2f;
    public Vector3 swayAxis = Vector3.up;

    private float swayTime = 0f;
    private float direction = 1f; // 左右の往復制御（1 or -1）
    private Vector3 centerPos;
    private float currentOffset = 0f;

    void Start()
    {
        if (player == null)
            player = Camera.main.transform;

        // 正面5mが移動の中央基準点
        centerPos = transform.position;

        // 水平方向の初期オフセット（ランダムでもOK）
        currentOffset = Random.Range(-moveRange, moveRange);
    }

    void Update()
    {
        // 左右の往復移動（currentOffsetを動かす）
        currentOffset += direction * moveSpeed * Time.deltaTime;

        // 範囲外に出たら反転
        if (Mathf.Abs(currentOffset) > moveRange)
        {
            direction *= -1f;
            currentOffset = Mathf.Clamp(currentOffset, -moveRange, moveRange);
        }

        

        // 上下のゆらゆら
        swayTime += Time.deltaTime;
        Vector3 swayOffset = swayAxis.normalized * Mathf.Sin(swayTime * swayFrequency) * swayAmplitude;

        // ワールド右方向で左右に動く（Y揺れ込み）
        transform.position = centerPos + Vector3.right * currentOffset + swayOffset;
    }
}
