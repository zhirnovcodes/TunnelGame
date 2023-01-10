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
        if (bezier.Point0 == null || bezier.Point1 == null || bezier.Point2 == null || bezier.Point3 == null)
        {
            return new BezierPoint();
        }

        // pos
        var p0 = Vector3.Lerp(bezier.Point0.position, bezier.Point1.position, t);
        var p1 = Vector3.Lerp(bezier.Point1.position, bezier.Point2.position, t);
        var p2 = Vector3.Lerp(bezier.Point2.position, bezier.Point3.position, t);

        var p3 = Vector3.Lerp(p0, p1, t);
        var p4 = Vector3.Lerp(p1, p2, t);

        var p5 = Vector3.Lerp(p3, p4, t);

        // rotation
        var q0 = (p4 - p3).normalized;

        return new BezierPoint { Position = p5, Rotation = Quaternion.LookRotation(q0) };
    }

    public static BezierPoint Lerp3(this Bezier bezier, float t)
    {
        if (bezier.Point0 == null || bezier.Point1 == null || bezier.Point2 == null)
        {
            return new BezierPoint();
        }

        // pos
        var p0 = Vector3.Lerp(bezier.Point0.position, bezier.Point1.position, t);
        var p1 = Vector3.Lerp(bezier.Point1.position, bezier.Point2.position, t);

        var p3 = Vector3.Lerp(p0, p1, t);

        // rotation
        var q0 = (p1 - p0).normalized;

        return new BezierPoint { Position = p3, Rotation = Quaternion.LookRotation(q0) };
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
        for (int i = 0; i < 4; i++)
        {
            var t = bezier.GetTransform(i);
            if (t == null)
            {
                if (i < 3)
                {
                    return;
                }
            }
            matrix.SetRow(i, t == null ? Vector3.zero : t.position);
        }
        block.SetMatrix(propertyNameId.Value, matrix);
        renderer.SetPropertyBlock(block);
    }
}
