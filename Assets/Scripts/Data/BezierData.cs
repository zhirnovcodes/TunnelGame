using System.Collections;
using UnityEngine;

[System.Serializable]
public struct BezierData
{
    public Vector3 P0;
    public Vector3 P1;
    public Vector3 P2;
    public Vector3 P3;

    public Vector3 Up;

    public int PointsCount;

    public float Length; // TODO float?

    public BezierData(Vector3 p0, Vector3 p1)
    {
        P0 = p0;
        P1 = p1;
        P2 = Vector3.zero;
        P3 = Vector3.zero;
        PointsCount = 2;
        Length = 0;
        Up = Vector3.up;
    }

    public BezierData(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        P0 = p0;
        P1 = p1;
        P2 = p2;
        P3 = Vector3.zero;
        PointsCount = 3;
        Length = 0;
        Up = Vector3.up;
    }

    public BezierData(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        P0 = p0;
        P1 = p1;
        P2 = p2;
        P3 = p3;
        PointsCount = 4;
        Length = 0;
        Up = Vector3.up;
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
            Up = Up,
            PointsCount = Mathf.Max(PointsCount, bezierTo.PointsCount)
        };
    }


    public PositionRotation Lerp(float t)
    {
        switch (PointsCount)
        {
            case 0: return new PositionRotation { Position = Vector3.zero, Rotation = Quaternion.identity };
            case 1: return new PositionRotation { Position = P0, Rotation = Quaternion.identity };
            case 2: return BezierExtentions.LerpBezier(P0, P1, t, Up);
            case 3: return BezierExtentions.LerpBezier(P0, P1, P2, t, Up);
            case 4: return BezierExtentions.LerpBezier(P0, P1, P2, P3, t, Up);
        }

        throw new System.NotImplementedException();

    }

    public BezierData AddPositionRotation(PositionRotation position)
    {
        var p0 = position.Rotation * P0 + position.Position;
        var p1 = position.Rotation * P1 + position.Position;
        var p2 = position.Rotation * P2 + position.Position;
        var p3 = position.Rotation * P3 + position.Position;

        var up = position.Rotation * Up;

        return new BezierData { P0 = p0, P1 = p1, P2 = p2, P3 = p3, Length = Length, PointsCount = PointsCount, Up = up };
    }

    public void Scale(float value)
    {
        P0 *= value;
        P1 *= value;
        P2 *= value;
        P3 *= value;
        Length *= value;
    }

    public void CalculateLength(int pointsCount)
    {
        Length = CalculateArcLength(0, 1, pointsCount);
    }

    public float CalculateArcLength(float tStart, float tEnd, int pointsCount)
    {
        var s = 0f;
        var pBefore = LerpPosition(tStart);
        var d = (tEnd - tStart) / pointsCount;

        for (float i = tStart + d; i < tEnd; i += d)
        {
            var p = LerpPosition(i);
            s += (pBefore - p).magnitude;
            pBefore = p;
        }

        var p1 = LerpPosition(tEnd);

        s += (pBefore - p1).magnitude;

        return s;
    }

    public Vector3 LerpPosition(float t)
    {
        switch (PointsCount)
        {
            case 0: return Vector3.zero;
            case 1: return P0;
            case 2: return BezierExtentions.LerpBezierPosition(P0, P1, t);
            case 3: return BezierExtentions.LerpBezierPosition(P0, P1, P2, t);
            case 4: return BezierExtentions.LerpBezierPosition(P0, P1, P2, P3, t);
        }

        throw new System.NotImplementedException();
    }


    public float GetTFromLength(float length, float tStart = 0, int arcLengthPiecesCount = 5, int maxDifferentiationsCount = 4)
    {
        var tEnd = tStart + length / Length;
        var iteration = 0;

        return PresiceTFromLength(length, tStart, tEnd, ref iteration, maxDifferentiationsCount, arcLengthPiecesCount);
    }

    private float PresiceTFromLength(float length, float tStart, float tEnd, ref int iteration, int maxDifferentiationsCount, int arcLengthPiecesCount = 5)
    {
        const float presicion = 0.0001f;

        if (iteration > maxDifferentiationsCount)
        {
            return tEnd;
        }
        iteration++;

        var magnitude = CalculateArcLength(tStart, tEnd, arcLengthPiecesCount);

        if (Mathf.Abs(length - magnitude) < presicion)
        {
            return tEnd;
        }

        tEnd = tStart + (tEnd - tStart) * length / magnitude;

        return PresiceTFromLength(length, tStart, tEnd, ref iteration, maxDifferentiationsCount);
    }
}
