using System.Collections.Generic;
using UnityEngine;

public class BulletLineCalc
{
    public bool IsRayHit { get; private set; } = false;
    public int HitTimeIndex { get; private set; } = 0;
    public GameObject HitObject { get; private set; }
    private float _drawTime; // 何秒後まで描画するか
    private int _segment; // 線の点の数
    private float _timeStep; // segment間の時間
    private float _gravity;

    public BulletLineCalc(float drawTime, int segment)
    {
        _drawTime = drawTime;
        _segment = segment;

        _timeStep = _drawTime / _segment;
        _gravity = Physics2D.gravity.y * -1;
    }

    /// <summary>
    /// BulletLineCalc.cs 放物線の弾道計算
    /// _segment分の弾道の瞬間位置を返り値で渡す
    /// </summary>

    public Vector3[] CalcLine(Vector2 fromPos, Vector2 velocity, int remainSegment = 0)
    {
        List<Vector3> arc = new List<Vector3>();
        IsRayHit = false;

        // 放物線の開始地点
        Vector2 currentPos = fromPos;
        arc.Add(new Vector3(currentPos.x, currentPos.y, 0f));

        int drawSegment;
        if (remainSegment == 0) drawSegment = _segment;
        else drawSegment = remainSegment;

        for (int i = 1; i <= drawSegment; i++)
        {
            float t = _timeStep * i;

            // 時間tにおける座標の計算
            float x = velocity.x * t;
            float y = (velocity.y * t) - (0.5f * _gravity * t * t);
            Vector2 nextPos = new Vector2(x, y) + fromPos;

            Vector2 dir = (nextPos - currentPos).normalized;
            float distance = Vector2.Distance(nextPos, currentPos);

            RaycastHit2D hit = Physics2D.Raycast(currentPos, dir, distance);
            Debug.DrawRay(currentPos, dir * distance, Color.red);

            if (hit.collider != null)
            {
                // 衝突時は描画をやめる
                arc.Add(new Vector3(hit.point.x, hit.point.y, 0f));
                IsRayHit = true;
                // 衝突したオブジェクトを返す
                HitObject = hit.collider.gameObject;
                HitTimeIndex = i;
                break;
            }

            arc.Add(new Vector3(nextPos.x, nextPos.y, 0f));
            currentPos = nextPos;
        }

        return arc.ToArray();
    }


}
