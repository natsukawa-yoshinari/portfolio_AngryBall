using UnityEngine;

public class OutPortal : MonoBehaviour
{
    [SerializeField]
    private GameObject _outPortal;
    [SerializeField]
    private LineRenderer _lineRenderer;

    private BulletLineCalc _bulletLineCalc;

    private void Start()
    {
        _bulletLineCalc = new BulletLineCalc(3f, 30);
    }

    public void DeletePortalLine()
    {
        _lineRenderer.positionCount = 0;
    }

    public void DrawOutPortalLine(Vector2 velocity, int remainSegment)
    {
        Vector2 from = _outPortal.transform.position;
        Vector3[] arc = _bulletLineCalc.CalcLine(from, velocity, remainSegment);

        _lineRenderer.positionCount = arc.Length;
        _lineRenderer.SetPositions(arc);

        // Rayが当たった
        if (_bulletLineCalc.IsRayHit)
        {
        }
        // Ray当たらない
        else
        {
        }
    }
}
