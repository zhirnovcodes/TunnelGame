using System.Collections.Generic;
using UnityEngine;

public class TunnelSplineModel : MonoBehaviour
{
    private BezierTunnelData TunnelData = new BezierTunnelData();

    public int MinIndex => TunnelData.StartIndex;

    public int MaxIndex => MinIndex + Count - 1;

    public int Count => TunnelData.Spline.BezierList.Count;

    public float LengthOffset => TunnelData.LengthOffset;

    public float SplineLength => TunnelData.Spline.Length;

    private void Awake()
    {
    }

    public void Append(BezierData data)
    {
        var positionBefore = TunnelData.Spline.Lerp(Count);
        var newBezier = data.AddPositionRotation( positionBefore);

        TunnelData.Spline.BezierList.Add(newBezier);
        TunnelData.Spline.Length += newBezier.Length;
    }

    public void Pop()
    {
        if (Count == 0)
        {
            return;
        }
        TunnelData.StartIndex++;
        TunnelData.LengthOffset += TunnelData.Spline.BezierList[0].Length;
        TunnelData.Spline.Length -= TunnelData.Spline.BezierList[0].Length;
        TunnelData.Spline.BezierList.RemoveAt(0);
    }

    public PositionRotation GetWorldPositionRotation(SplinePositionData data)
    {
        var t = data.Position.z - MinIndex;
        var point = TunnelData.Spline.Lerp(t);
        var xyPosition = point.Rotation * new Vector3(data.Position.x, data.Position.y, 0);

        point.Position += xyPosition;

        return point;
    }

    public SplinePositionData MovePosition(SplinePositionData position, Vector3 speed)
    {
        var breakCount = 0;
        var x = position.Position.x + speed.x;
        var y = position.Position.y + speed.y;
        var z = SplineMovePosition(TunnelData.Spline, position.Position.z - TunnelData.StartIndex, speed.z, ref breakCount) + TunnelData.StartIndex;

        position.Position = new Vector3(x, y, z);

        return position;
    }

    private float SplineMovePosition(BezierSplineData spline, float t, float speed, ref int breakCount)
    {
        if (++breakCount > 1000)
        {
            throw new System.Exception();
        }


        if (Mathf.Approximately(speed, 0) || spline.BezierList.Count < 0)
        {
            return t;
        }

        var indexCurrent = Mathf.FloorToInt(t);

        if (speed < 0 && (t % 1) <= 0)
        {
            indexCurrent--;
            t = 1;
        }
        else
        {
            t %= 1;
        }

        if (indexCurrent >= 0 && indexCurrent < spline.BezierList.Count )
        {
            t = BezierMovePosition(spline.BezierList[indexCurrent], t, ref speed) + indexCurrent;

            return SplineMovePosition(spline, t, speed, ref breakCount);
        }


        return Mathf.Clamp(indexCurrent, 0, spline.BezierList.Count);
    }

    private float BezierMovePosition(BezierData bezier, float t, ref float speed)
    {
        const float tolerance = 0.00001f;

        t = (t >= 0 && t <= 1) ? t : (t % 1);

        var speedSign = Mathf.Sign(speed);
        var speedToT = speed / bezier.Length;
        var tNew = Mathf.Clamp01(t + speedToT);
        var deltaT = tNew - t;
        var deltaLength = deltaT * bezier.Length;

        speed -= deltaLength;
        speed = (Mathf.Abs(speed) < tolerance || !Mathf.Approximately(speedSign, Mathf.Sign(speed))) ? 0 : speed;

        return t + deltaT;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Count; i++)
        {
            var bezier = TunnelData.Spline.BezierList[i];
            var pr = bezier.Lerp(0);

            Gizmos.DrawRay(new Ray(pr.Position, bezier.Up));
        }
    }
}
