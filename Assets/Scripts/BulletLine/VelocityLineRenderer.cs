using UnityEngine;

public class VelocityLineRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    private GameObject _startVelocityPoint;
    private float _maxDistance = 5f;
    private SpriteRenderer _startPointSR;

    // MaterialのPropertyにアクセスするID
    private static readonly int SecondColorID =
        Shader.PropertyToID("_SecondColor");
    private static readonly int RedContrastID =
        Shader.PropertyToID("_RedContrast");
    private static readonly int GreenContrastID =
        Shader.PropertyToID("_GreenContrast");

    private void Start()
    {
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _startPointSR = _startVelocityPoint.GetComponent<SpriteRenderer>();
    }

    public void DeleteLine()
    {
        _lineRenderer.positionCount = 0;
        _startVelocityPoint.SetActive(false);
    }

    public void DrawLine(Vector2 from, Vector2 to, Vector2 velocity)
    {
        _startVelocityPoint.SetActive(true);
        _startVelocityPoint.transform.position = from;
        _lineRenderer.positionCount = 2;

        // Shaderで思い通りにできなかったので、fromが後にくる
        _lineRenderer.SetPosition(1, from);
        _lineRenderer.SetPosition(0, to);
        UpdateLineColor(from, to);

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        _startVelocityPoint.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90f));
    }

    private void UpdateLineColor(Vector2 from, Vector2 to)
    {
        float distance = Vector2.Distance(from, to);

        float normalizeDist = (distance / _maxDistance);

        // 2以上にならないように固定
        // 完全に2にすると、Shaderの部分でおかしくなっちゃう
        if (normalizeDist > 2) normalizeDist = 1.9999f;

        Color c;

        // 0 ~ 1まで
        if (normalizeDist < 1)
        {
            // 赤コントラストは0のまま、緑コントラストを動かす
            _lineRenderer.material.SetFloat(RedContrastID, 0);
            _lineRenderer.material.SetFloat(GreenContrastID, normalizeDist);

            // rgbのrだけ0 ~ 1にして黄色(※緑→黄→赤)にする
            c = new Color(normalizeDist, 1, 0);
        }
        // 1 ~ 2まで
        else if (normalizeDist >= 1)
        {
            // 0 ~ 1のスケールにするため1引く
            normalizeDist -= 1f;

            // コントラストの操作
            _lineRenderer.material.SetFloat(RedContrastID, normalizeDist);
            _lineRenderer.material.SetFloat(GreenContrastID, 0.9999f);

            if (normalizeDist > 1) return;
            // rgbのgだけ1 ~ 0にして赤色(※緑→黄→赤)にする
            c = new Color(1, (1 - normalizeDist), 0);
        }
        else c = Color.white;

        _lineRenderer.material.SetColor(SecondColorID, c);
        _startPointSR.color = c;
    }
}
