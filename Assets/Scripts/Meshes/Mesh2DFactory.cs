using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mesh2DFactory
{
    public static void FillCircleMesh2D(Mesh2D mesh, int slices, bool shadedInside, float radius = 1, float spinDegrees = 0)
    {
        mesh.Clear();

        var spinRad = spinDegrees / 360f;

        for (int s = 0; s <= slices; s++)
        {
            float u = s / (float)slices;
            var radians = MathP.TAU * (u + spinRad);
            var direction = Vector2.up.Rotate(radians);
            var position = direction * radius;
            var normal = direction.normalized * ( shadedInside ? - 1 : 1);

            mesh.Vertices.Add(position);
            mesh.Normals.Add(normal);
            mesh.Us.Add(u);

            if (shadedInside)
            {
                mesh.Lines.Add((s + 1) % (slices + 1));
                mesh.Lines.Add(s);
            }
            else
            {
                mesh.Lines.Add(s);
                mesh.Lines.Add((s + 1) % (slices + 1));
            }
        }
    }
}
