using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class BezierExtentions
{
    /*public static Bezier GenerateBezier(Bezier current, Bezier previous, float angleRandom)
    {

    }*/

    public static BezierPoint Lerp(this Bezier bezier, float t)
    {
        if (bezier.Point0 == null || bezier.Point1 == null || bezier.Point2 == null)
        {
            return new BezierPoint();
        }

        if (bezier.Point3 == null)
        {
            return LerpBezier(bezier.Point0.position, bezier.Point1.position, bezier.Point2.position, t);
        }

        return LerpBezier(bezier.Point0.position, bezier.Point1.position, bezier.Point2.position, bezier.Point3.position, t);
    }
    
    public static BezierPoint Lerp(this BezierData bezier, float t)
    {
        if (bezier.PointsCount == 4)
        {
            return LerpBezier(bezier.P0, bezier.P1, bezier.P2, bezier.P3, t);
        }
        return LerpBezier(bezier.P0, bezier.P1, bezier.P2, t);

    }

    public static BezierData Slerp(BezierData bezier1, BezierData bezier2, float t)
    {
        return new BezierData()
        {
            P0 = Vector3.Slerp(bezier1.P0, bezier2.P0, t),
            P1 = Vector3.Slerp(bezier1.P1, bezier2.P1, t),
            P2 = Vector3.Slerp(bezier1.P2, bezier2.P2, t),
            P3 = Vector3.Slerp(bezier1.P3, bezier2.P3, t)
        };
    }

    public static BezierPoint LerpBezier(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float t)
    {
        var p0 = Vector3.Lerp(point0, point1, t);
        var p1 = Vector3.Lerp(point1, point2, t);
        var p2 = Vector3.Lerp(point2, point3, t);

        var p3 = Vector3.Lerp(p0, p1, t);
        var p4 = Vector3.Lerp(p1, p2, t);

        var p5 = Vector3.Lerp(p3, p4, t);

        // rotation
        var q0 = (p4 - p3).normalized;

        return new BezierPoint { Position = p5, Rotation = Quaternion.LookRotation(q0) };
    }

    public static BezierPoint LerpBezier(Vector3 point0, Vector3 point1, Vector3 point2, float t)
    {
        var p0 = Vector3.Lerp(point0, point1, t);
        var p1 = Vector3.Lerp(point1, point2, t);

        var p3 = Vector3.Lerp(p0, p1, t);

        // rotation
        var q0 = (p1 - p0).normalized;

        return new BezierPoint { Position = p3, Rotation = Quaternion.LookRotation(q0) };
    }

    public static BezierPoint Lerp(this BezierData4 bezier, float t)
    {
        return LerpBezier(bezier.P0, bezier.P1, bezier.P2, bezier.P3, t);
    }

    public static BezierPoint Lerp(this BezierData3 bezier, float t)
    {
        return LerpBezier(bezier.P0, bezier.P1, bezier.P2, t);
    }

    public static BezierData ToBezierData(this BezierObject o)
    {
        var pointsCount = o.Bezier.Point3 == null ? 3 : 4;
        return new BezierData
        {
            P0 = o.Bezier.Point0.position,
            P1 = o.Bezier.Point1.position,
            P2 = o.Bezier.Point2.position,
            P3 = pointsCount == 3 ? Vector3.zero : o.Bezier.Point3.position,
            PointsCount = pointsCount
        };
    }

    public static void SetPositionRotation(this Bezier bezier, Vector3 position, Quaternion rotation, int index)
    {
        Transform current;
        switch (index)
        {
            case 0:
                {
                    current = bezier.Point0;
                    break;
                }
            case 1:
                {
                    current = bezier.Point1;
                    break;
                }
            case 2:
                {
                    current = bezier.Point2;
                    break;
                }
            case 3:
                {
                    current = bezier.Point3;
                    break;
                }
            default:
                {
                    throw new IndexOutOfRangeException();
                }
        }

        current.position = position;
        current.rotation = rotation;
    }

    public static Transform GetTransform(this Bezier bezier, int index)
    {
        switch (index)
        {
            case 0:
                {
                    return bezier.Point0;
                }
            case 1:
                {
                    return bezier.Point1;
                }
            case 2:
                {
                    return bezier.Point2;
                }
            case 3:
                {
                    return bezier.Point3;
                }
            default:
                {
                    throw new IndexOutOfRangeException();
                }
        }
    }

    public static void DrawBezier(Bezier bezier, bool shouldDrawHandles = true, float t = 0)
    {
#if UNITY_EDITOR
        if (bezier.Point0 == null || bezier.Point1 == null || bezier.Point2 == null)
        {
            return;
        }

        if (bezier.Point3 == null)
        {
            UnityEditor.Handles.DrawBezier(
                bezier.Point0.position,
                bezier.Point2.position,
                bezier.Point0.position,
                bezier.Point1.position,
                Color.white, Texture2D.whiteTexture, 2);
        }
        else
        {

            UnityEditor.Handles.DrawBezier(
                bezier.Point0.position,
                bezier.Point3.position,
                bezier.Point1.position,
                bezier.Point2.position,
                Color.white, Texture2D.whiteTexture, 2);

            if (shouldDrawHandles)
            {
                var point = bezier.Lerp(t);
                UnityEditor.Handles.PositionHandle(point.Position, point.Rotation);

            }
        }
#endif
    }

    public static void SendBezierToShader(this Bezier bezier, MeshRenderer renderer, ref MaterialPropertyBlock block, ref int? propertyNameId)
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
        }
        if (propertyNameId == null)
        {
            propertyNameId = Shader.PropertyToID("_BezierNodes");
        }

        var matrix = new Matrix4x4();
        int pointsCount = 0;
        for (int i = 0; i < 4; i++)
        {
            var t = bezier.GetTransform(i);
            if (t == null)
            {
                if (i < 3)
                {
                    return;
                }
                break;
            }
            else
            {
                pointsCount++;
            }
            matrix.SetRow(i, t == null ? Vector3.zero : t.position);
        }

        var lastRow = matrix.GetRow(3);
        lastRow.w = pointsCount;
        matrix.SetRow(3, lastRow);

        block.SetMatrix(propertyNameId.Value, matrix);
        renderer.SetPropertyBlock(block);
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
}
