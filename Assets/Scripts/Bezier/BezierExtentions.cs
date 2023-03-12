using UnityEngine;

public static class BezierExtentions
{
    public static PositionRotation LerpBezier(Vector3 point0, Vector3 point1, float t)
    {
        var p0 = Vector3.Lerp(point0, point1, 0);

        var p1 = Vector3.Lerp(point0, point1, t);

        // rotation
        var q0 = (p1 - p0).normalized;

        return new PositionRotation { Position = p1, Rotation = Quaternion.LookRotation(q0) };
    }

    public static PositionRotation LerpBezier(Vector3 point0, Vector3 point1, Vector3 point2, float t)
    {
        var p0 = Vector3.Lerp(point0, point1, t);
        var p1 = Vector3.Lerp(point1, point2, t);

        var p3 = Vector3.Lerp(p0, p1, t);

        // rotation
        var q0 = (p1 - p0).normalized;

        return new PositionRotation { Position = p3, Rotation = Quaternion.LookRotation(q0) };
    }

    public static PositionRotation LerpBezier(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float t)
    {
        var p0 = Vector3.Lerp(point0, point1, t);
        var p1 = Vector3.Lerp(point1, point2, t);
        var p2 = Vector3.Lerp(point2, point3, t);

        var p3 = Vector3.Lerp(p0, p1, t);
        var p4 = Vector3.Lerp(p1, p2, t);

        var p5 = Vector3.Lerp(p3, p4, t);

        // rotation
        var q0 = (p4 - p3).normalized;

        return new PositionRotation { Position = p5, Rotation = Quaternion.LookRotation(q0) };
    }

    public static void SendBezierToShader(BezierData bezier, Material material, ref int? propertyNameId)
    {
        if (propertyNameId == null)
        {
            propertyNameId = Shader.PropertyToID("_BezierNodes");
        }

        var matrix = new Matrix4x4();
        matrix.SetRow(0, bezier.P0);
        matrix.SetRow(1, bezier.P1);
        matrix.SetRow(2, bezier.P2);
        var lastRow = new Vector4(bezier.P3.x, bezier.P3.y, bezier.P3.z, bezier.PointsCount);
        matrix.SetRow(3, lastRow);

        material.SetMatrix(propertyNameId.Value, matrix);
    }

    public static void SendBezierToShader(BezierData bezier, MeshRenderer renderer, ref MaterialPropertyBlock block, ref int? propertyNameId)
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
        }
        if (propertyNameId == null)
        {
            propertyNameId = Shader.PropertyToID("_BezierNodes");
        }

        var matrix = bezier.ToMatrix4x4();

        block.SetMatrix(propertyNameId.Value, matrix);
        renderer.SetPropertyBlock(block);
    }
}
