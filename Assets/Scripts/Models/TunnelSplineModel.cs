using System.Collections.Generic;
using UnityEngine;

public class TunnelSplineModel : MonoBehaviour
{
    private BezierTunnelData TunnelData = new BezierTunnelData();

    public int MinIndex => TunnelData.StartIndex;
    public int MaxIndex => MinIndex + Count - 1;
    public int Count => TunnelData.Spline.BezierList.Count;

    private void Awake()
    {
    }

    public void Append(BezierData data)
    {
        var positionBefore = TunnelData.Spline.Lerp(Count);
        var newBezier = data.AddPositionRotation( positionBefore);

        TunnelData.Spline.BezierList.Add(newBezier);
    }

    public void Pop()
    {
        if (Count == 0)
        {
            return;
        }
        TunnelData.StartIndex++;
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
        var z = Mathf.Clamp(position.Position.z, TunnelData.StartIndex, TunnelData.StartIndex + TunnelData.Spline.BezierList.Count);
        var indexCurrent = Mathf.FloorToInt(z);
        var speedSign = Mathf.Sign(speed.z);
        var speedZ = speed.z;
        var breakCount = 0;

        while (indexCurrent >= MinIndex && indexCurrent <= MaxIndex && !Mathf.Approximately(speedZ, 0)) 
        {
            if (++breakCount > 1000)
            {
                throw new System.Exception();
            }

            if (speedSign > 0)
            {
                var tLocal = z % 1;

                z = indexCurrent + BezierMovePosition(TunnelData.Spline.BezierList[indexCurrent - MinIndex], tLocal, ref speedZ);
                indexCurrent = Mathf.FloorToInt(z);
            }
            if (speedSign < 0)
            {
                var tLocal = z % 1;

                z = indexCurrent + BezierMovePosition(TunnelData.Spline.BezierList[indexCurrent - MinIndex], tLocal, ref speedZ);
                indexCurrent = Mathf.FloorToInt(z);
            }
        }

        var x = position.Position.x + speed.x;
        var y = position.Position.y + speed.y;

        position.Position = new Vector3(x, y, z);

        return position;
    }

    private float BezierMovePosition(BezierData bezier, float t, ref float speed)
    {
        const float tolerance = 0.00001f;

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
