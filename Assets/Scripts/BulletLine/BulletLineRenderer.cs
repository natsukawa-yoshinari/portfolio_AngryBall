using UnityEngine;

public class BulletLineRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _lineRenderer;
    [SerializeField]
    private int _drawLineTime = 5;
    [SerializeField]
    private int _drawSegment = 30;

    private const int LAYER_PORTAL_IN = 10;
    private OutPortal _portalManager;
    private BulletLineCalc _bulletLineCalc;

    private void Start()
    {
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _bulletLineCalc = new BulletLineCalc(_drawLineTime, _drawSegment);
    }

    public void Initialize(OutPortal portalManager)
    {
        _portalManager = portalManager;
    }

    public void DeleteLine()
    {
        _lineRenderer.positionCount = 0;
        _portalManager.DeletePortalLine();
    }

    /// <summary>
    /// BulletLineRenderer.cs
    /// BulletLineCalcで取得した弾道をLineRendererに移す
    /// 弾道のRayの処理も行う
    /// </summary>

    public void DrawLine(Vector2 from, Vector2 velocity)
    {
        Vector3[] arc = _bulletLineCalc.CalcLine(from, velocity);

        _lineRenderer.positionCount = arc.Length;
        _lineRenderer.SetPositions(arc);

        // Rayが当たった
        if (_bulletLineCalc.IsRayHit)
        {
            if (_bulletLineCalc.HitObject == null) return;
            if (_bulletLineCalc.HitObject.layer != LAYER_PORTAL_IN)
            {
                _portalManager.DeletePortalLine();
                return;
            }

            // ポータルに当たったらVelocityを引き継ぐ
            float t = (_bulletLineCalc.HitTimeIndex) * (_drawLineTime / (float)_drawSegment);
            float vx = velocity.x;
            float vy = velocity.y + (Physics2D.gravity.y * t);
            Vector2 hitTimeVelocity = new Vector2(vx, vy);
            int remainSegment = _drawSegment - _bulletLineCalc.HitTimeIndex;

            _portalManager.DrawOutPortalLine(hitTimeVelocity, remainSegment);
        }
        // Ray当たらない
        else
        {
            _portalManager.DeletePortalLine();
        }
    }
}
