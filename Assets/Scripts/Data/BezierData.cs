using System.Collections;
using UnityEngine;

[System.Serializable]
public struct BezierData
{
    public Vector3 P0;
    public Vector3 P1;
    public Vector3 P2;
    public Vector3 P3;

    public int PointsCount;

    public float Length;

    public BezierData(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        P0 = p0;
        P1 = p1;
        P2 = p2;
        P3 = Vector3.zero;
        PointsCount = 3;
        Length = 0;
    }

    public BezierData(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        P0 = p0;
        P1 = p1;
        P2 = p2;
        P3 = p3;
        PointsCount = 4;
        Length = 0;
    }
    public Matrix4x4 ToMatrix4x4()
    {
        var matrix = new Matrix4x4();
        matrix.SetRow(0, P0);
        matrix.SetRow(1, P1);
        matrix.SetRow(2, P2);
        var lastRow = new Vector4(P3.x, P3.y, P3.z, PointsCount);
        matrix.SetRow(3, lastRow);

        return matrix;
    }

    public BezierData SlerpToBezier(BezierData bezierTo, float t)
    {
        return new BezierData()
        {
            P0 = Vector3.Slerp(P0, bezierTo.P0, t),
            P1 = Vector3.Slerp(P1, bezierTo.P1, t),
            P2 = Vector3.Slerp(P2, bezierTo.P2, t),
            P3 = Vector3.Slerp(P3, bezierTo.P3, t),
            Length = Mathf.Lerp(Length, bezierTo.Length, t), 
            PointsCount = Mathf.Max(PointsCount, bezierTo.PointsCount)
        };
    }


    public PositionRotation Lerp(float t)
    {
        switch (PointsCount)
        {
            case 0: return new PositionRotation { Position = Vector3.zero, Rotation = Quaternion.identity };
            case 1: return new PositionRotation { Position = P0, Rotation = Quaternion.identity };
            case 2: return BezierExtentions.LerpBezier(P0, P1, t);
            case 3: return BezierExtentions.LerpBezier(P0, P1, P2, t);
            case 4: return BezierExtentions.LerpBezier(P0, P1, P2, P3, t);
        }

        throw new System.NotImplementedException();

    }

    public BezierData AddPositionRotation(PositionRotation position)
    {
        var d0 = P1 - P0;
        var d1 = P2 - P0;
        var d2 = P3 - P2;

        d0 = position.Rotation * d0;
        d1 = position.Rotation * d1;
        d2 = position.Rotation * d2;

        var p0 = P0 + position.Position;
        var p1 = p0 + d0;
        var p2 = p0 + d1;
        var p3 = p0 + d2;

        return new BezierData { P0 = p0, P1 = p1, P2 = p2, P3 = p3, Length = Length, PointsCount = PointsCount };
    }

    public void Scale(float value)
    {
        P0 *= value;
        P1 *= value;
        P2 *= value;
        P3 *= value;
        Length *= value;
    }
}
