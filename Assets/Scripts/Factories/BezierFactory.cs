using UnityEngine;

public static class BezierFactory
{
    public static BezierData BuildStraight()
    {
        var bezier = new BezierData ( Vector3.zero, Vector3.forward / 2f, Vector3.forward) { Length = 1 };
        return bezier;
    }
    public static BezierData BuildCurvedOne(Vector3 direction)
    {
        var r = 2 / Mathf.PI;
        var bezier = new BezierData(Vector3.zero, Vector3.forward * r, (Vector3.forward + direction) * r) { Length = 1 };
        return bezier;
    }
    public static BezierData BuildCurved90(Vector3 direction)
    {
        var r = 1;
        var l = Mathf.PI * r / 2f;
        var bezier = new BezierData(Vector3.zero, Vector3.forward * r, (Vector3.forward + direction) * r) { Length = l };

        return bezier;
    }
}
