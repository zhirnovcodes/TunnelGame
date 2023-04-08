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
    public static BezierData BuildCurved90(Vector3 direction, float radius = 1, float arcLength = 0.01f)
    {
        var l = Mathf.PI * radius / 2f;
        var bezier = new BezierData(Vector3.zero, Vector3.forward * radius, (Vector3.forward + direction) * radius);
        bezier.CalculateLength(Mathf.RoundToInt(l / arcLength));

        return bezier;
    }
    public static BezierData BuildCurved(Vector3 direction, float radius, float angleRadians)
    {
        var l = angleRadians * radius;
        var bezier = new BezierData(Vector3.zero, Vector3.forward * radius, (Vector3.forward + direction) * radius) { Length = l };

        return bezier;
    }
    public static BezierData BuildHillUp(float height)
    {
        return BuildWiggled(Vector3.up * height);
    }
    public static BezierData BuildHillDown(float height)
    {
        return BuildWiggled(Vector3.down * height);
    }
    public static BezierData BuildWiggledLeft(float width)
    {
        return BuildWiggled(Vector3.left * width);
    }
    public static BezierData BuildWiggledRight(float width)
    {
        return BuildWiggled(Vector3.right * width);
    }
    public static BezierData BuildWiggled(Vector3 side)
    {
        var p0 = Vector3.zero;
        var p1 = Vector3.forward * 0.5f;
        var p2 = p1 + side;
        var p3 = Vector3.forward + side;

        var bezier = new BezierData(p0, p1, p2, p3) { };
        bezier.CalculateLength(1000);

        return bezier;
    }
}
