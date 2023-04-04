using UnityEngine;

public static class BezierExtentions
{
    public static PositionRotation LerpBezier(Vector3 point0, Vector3 point1, float t, Vector3? up = null)
    {
        up = up ?? Vector3.up;

        var p1 = Vector3.Lerp(point0, point1, t);

        // rotation
        var q0 = (point1 - point0).normalized;

        return new PositionRotation { Position = p1, Rotation = Quaternion.LookRotation(q0, up.Value) };
    }

    public static PositionRotation LerpBezier(Vector3 point0, Vector3 point1, Vector3 point2, float t, Vector3? up = null)
    {
        up = up ?? Vector3.up;

        var p0 = Vector3.Lerp(point0, point1, t);
        var p1 = Vector3.Lerp(point1, point2, t);

        var p3 = Vector3.Lerp(p0, p1, t);

        // rotation
        var rotation = GetRotation (p1 - p0, point1 - point0, up.Value);

        return new PositionRotation { Position = p3, Rotation = rotation };
    }

    public static PositionRotation LerpBezier(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float t, Vector3? up = null)
    {
        up = up ?? Vector3.up;

        var p0 = Vector3.Lerp(point0, point1, t);
        var p1 = Vector3.Lerp(point1, point2, t);
        var p2 = Vector3.Lerp(point2, point3, t);

        var p3 = Vector3.Lerp(p0, p1, t);
        var p4 = Vector3.Lerp(p1, p2, t);

        var p5 = Vector3.Lerp(p3, p4, t);

        // rotation
        var rotation = GetRotation(p4 - p3, point1 - point0, up.Value);

        return new PositionRotation { Position = p5, Rotation = rotation };
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

    private static Quaternion GetRotation(Vector3 currentDirection, Vector3 startDirection, Vector3 up)
    {
        currentDirection = currentDirection.normalized;
        startDirection = startDirection.normalized;
        up = Quaternion.FromToRotation(startDirection, currentDirection) * up;

        return Quaternion.LookRotation(currentDirection, up);

    }

    public static void BendMeshWithBezier(Mesh3D mesh, BezierData bezier, SplineParametrizationMap parametrization = null)
    {
        for (int i = 0; i < mesh.Vertices.Count; i++)
        {
            var position = new Vector3( mesh.Vertices[i].x, mesh.Vertices[i].y, 0);
            var normal = mesh.Normals[i];
            var mestT = mesh.Uvs[i].y;
            var bezierT = parametrization == null ? mestT : parametrization.GetT(mestT * bezier.Length);
            var bezierPosition = bezier.Lerp(mestT >= 1 ? 1 : bezierT);

            var positionNew = bezierPosition.Rotation * position + bezierPosition.Position;
            var normalNew = bezierPosition.Rotation * normal;

            mesh.Vertices[i] = positionNew;
            mesh.Normals[i] = normalNew;
        }
    }

    public static Vector3 LerpBezierPosition(Vector3 point0, Vector3 point1, float t)
    {
        return Vector3.Lerp(point0, point1, t);
    }

    public static Vector3 LerpBezierPosition(Vector3 point0, Vector3 point1, Vector3 point2, float t)
    {
        float u = 1 - t;

        return u * u * point0 + 2 * u * t * point1 + t * t * point2;
    }

    public static Vector3 LerpBezierPosition(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float t)
    {
        float u = 1f - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;

        Vector3 result =
            u3 * point0 +
            3f * u2 * t * point1 +
            3f * u * t2 * point2 +
            t3 * point3;

        return result;
    }
}
