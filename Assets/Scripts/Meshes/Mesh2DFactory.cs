using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mesh2DFactory
{
    public static void CreateCircleMesh2D(Mesh2D mesh, int slices, bool shadedInside, float radius = 1)
    {
        mesh.Clear();

        for (int s = 0; s <= slices; s++)
        {
            float u = s / (float)slices;
            var radians = MathP.TAU * u;
            var direction = Vector2.up.RotateCounterClockWise(radians);
            var position = direction * radius;
            var normal = direction.normalized * ( shadedInside ? - 1 : 1);

            var nextIndex = (s + 1) % (slices + 1);

            var linePoint1 = shadedInside ? nextIndex : s;
            var linePoint2 = shadedInside ? s : nextIndex;

            mesh.Vertices.Add(position);
            mesh.Normals.Add(normal);
            mesh.Us.Add(u);

            mesh.Lines.Add(linePoint1);
            mesh.Lines.Add(linePoint2);
        }
    }
    public static void CreateStrictEdge(Mesh2D mesh, int slices, bool shadedInside)
    {
        mesh.Clear();

        var pointsCount = 0;

        for (int s0 = 0; s0 < slices; s0++)
        {
            var s1 = s0 + 1;

            var u0 = s0 / (float)slices;
            var u1 = s1 / (float)slices;

            var angle0 = MathP.TAU * u0;
            var angle1 = MathP.TAU * u1;

            var direction0 = Vector2.up.RotateCounterClockWise(angle0);
            var direction1 = Vector2.up.RotateCounterClockWise(angle1);

            var position0 = direction0;
            var position1 = direction1;

            var normal = (position1 - position0).RotateCounterClockWise(Mathf.PI) * (shadedInside ? -1 : 1);

            var i0 = pointsCount;
            var i1 = pointsCount + 1;

            pointsCount += 2;

            var index0 = shadedInside ? i1 : i0;
            var index1 = shadedInside ? i0 : i1;

            mesh.Vertices.Add(position0);
            mesh.Vertices.Add(position1);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            mesh.Us.Add(u0);
            mesh.Us.Add(u1);

            mesh.Lines.Add(index0);
            mesh.Lines.Add(index1);
        }
    }

    public static void CreateLine(Mesh2D mesh, int slices)
    {
        mesh.Clear();

        var point0 = Vector2.right / 2f;
        var point1 = Vector2.left / 2f;
        var normal = Vector2.up;

        for (int s = 0; s <= slices; s++)
        {
            float u = s / (float)slices;
            var position = Vector2.Lerp(point0, point1, u);

            mesh.Vertices.Add(position);
            mesh.Normals.Add(normal);
            mesh.Us.Add(u);

            if (s < slices)
            {
                mesh.Lines.Add(s);
                mesh.Lines.Add(s + 1);
            }
        } 
    }
}
