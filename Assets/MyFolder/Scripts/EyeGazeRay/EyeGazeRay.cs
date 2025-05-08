using UnityEngine;


namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class EyeGazeRay : MonoBehaviour
            {
                [Header("Ray Settings")]
                public float rayLength = 25f;
                public LineRenderer lineRenderer;

                private RaycastHit hit;
                private string currentLookTargetName;
                private float targetDistance;
                private bool isHitOtherObj = false;

                #region Ray:ブレ調整用
                private Vector3 smoothedDirection = Vector3.zero;
                public float smoothFactor = 0.15f; // 0〜1（値が小さいほど安定、ただし反応は遅くなる）
                private Vector3 drawOrigin, drawDirection;
                public float rayOriginOffset = 0.05f;
                #endregion

                #region Ray:太さ調整用
                public float midRayWidth = 0.01f;
                public float farRayWidth = 0.04f;
                #endregion


                #region 視線の取得用
                private Vector3 localGazeOrigin, localGazeDirection;
                private Vector3 rayOrigin, gazeDirection;
                #endregion 

                private void Start()
                {

                }

                void Update()
                {
                    // 視線取得
                    bool gotGaze = SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out localGazeOrigin, out localGazeDirection);
                    if (!gotGaze) return;


                    // ワールド空間のRay作成
                    rayOrigin = Camera.main.transform.position;
                    gazeDirection = Camera.main.transform.TransformDirection(localGazeDirection);

                    // Raycast
                    if (Physics.Raycast(rayOrigin, rayOrigin + gazeDirection * rayLength, out hit))
                    {
                        isHitOtherObj = true;
                        currentLookTargetName = hit.collider.gameObject.name;
                        targetDistance = hit.distance;
                        
                    }
                    else
                    {
                        isHitOtherObj = false;
                        targetDistance = rayLength;
                    }

                    // 可視化
                    DrawGazeRay(rayOrigin, gazeDirection);
                }

                private void DrawGazeRay(Vector3 origin, Vector3 direction)
                {
                    // 毎フレーム Update
                    drawOrigin = origin - Camera.main.transform.up * rayOriginOffset;

                    // スムージング
                    smoothedDirection = Vector3.Lerp(smoothedDirection, direction, smoothFactor);

                    if (lineRenderer != null)
                    {
                        lineRenderer.SetPosition(0, drawOrigin);
                        lineRenderer.SetPosition(1, origin + smoothedDirection * targetDistance);

                        lineRenderer.widthCurve = GenerateWidthCurve(targetDistance);
                    }

                    
                }

                private AnimationCurve GenerateWidthCurve(float totalDistance)
                {
                    // 正規化（0=始点, 1=終点）
                    float near = 0f;
                    float mid = Mathf.Clamp01(0.3f / totalDistance); // 30cm地点
                    float far = 1f;

                    AnimationCurve curve = new AnimationCurve();

                    // 始点（近すぎると見えないようにほぼ0）
                    curve.AddKey(near, 0f);
                    // 30cm地点：少し見える程度
                    curve.AddKey(mid, midRayWidth);
                    // 最後（遠く）：太め
                    curve.AddKey(far, farRayWidth);

                    return curve;
                }

            }
        }
    }
}
