using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierFactory
{
    public static BezierData BuildStraight()
    {
        var bezier2 = new BezierData { P0 = Vector3.zero, P1 = Vector3.forward / 2f, P2 = Vector3.forward, PointsCount = 3, Length = 1 };
        return bezier2;
    }
    public static BezierData BuildCurvedOne(Vector3 direction)
    {
        var r = 2 / Mathf.PI;
        var bezier0 = new BezierData { P0 = Vector3.zero, P1 = Vector3.forward * r, P2 = (Vector3.forward + direction) * r, PointsCount = 3, Length = 1 };
        return bezier0;
    }
    public static BezierData BuildCurved90(Vector3 direction)
    {
        var r = 1;
        var l = Mathf.PI * r / 2f;
        var bezier0 = new BezierData { P0 = Vector3.zero, P1 = Vector3.forward * r, P2 = (Vector3.forward + direction) * r, PointsCount = 3, Length = l };
        return bezier0;
    }
}
