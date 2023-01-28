using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TunnelDetailFactory
{
    public static TunnelDetailData BuildStraight()
    {
        var bezier2 = new BezierData { P0 = Vector3.zero, P1 = Vector3.forward / 2f, P2 = Vector3.forward, PointsCount = 3 };
        return new TunnelDetailData { Bezier = bezier2, Length = 1 };
    }
    public static TunnelDetailData BuildCurved(Vector3 direction)
    {
        var r = 2 / Mathf.PI;
        var bezier0 = new BezierData { P0 = Vector3.zero, P1 = Vector3.forward * r, P2 = (Vector3.forward + direction) * r, PointsCount = 3 };
        return new TunnelDetailData { Bezier = bezier0, Length = 1 };
    }
}
