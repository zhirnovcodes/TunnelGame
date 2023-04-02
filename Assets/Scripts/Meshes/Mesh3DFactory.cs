using UnityEngine;

public static class Mesh3DFactory
{
    public static Mesh3D CreateCapsule(Mesh2D mesh2D, int fragments, float roundEdge, ref Mesh3D mesh)
    {
        mesh = mesh ?? new Mesh3D();

        mesh.Clear();

        var verticesCount2D = mesh2D.Vertices.Count;

        for (int f = 0; f < fragments + 1; f++)
        {
            var t = Mathf.InverseLerp(0f, fragments, f);

            var bPoint = new PositionRotation
            {
                Position = Vector3.Lerp(Vector3.zero, Vector3.forward, t),
            };

            for (int s = 0; s < verticesCount2D; s++)
            {
                var positionLocal = bPoint.Rotation * mesh2D.Vertices[s];
                var positionGlobal = positionLocal + bPoint.Position;
                var rotation = bPoint.Rotation;
                var normal = rotation * mesh2D.Normals[s];
                var uv = new Vector2(mesh2D.Us[s], t);

                if (roundEdge > 0)
                {
                    if (uv.y <= roundEdge)
                    {
                        var rePosC = Vector3.forward * roundEdge;

                        var rePos0 = Vector3.zero;
                        var rePos1 = rePos0 + positionLocal;
                        var rePos2 = rePosC + positionLocal;

                        var reT = Mathf.InverseLerp(0, roundEdge, uv.y);

                        var rePosA0 = Vector3.Lerp(rePos0, rePos1, reT);
                        var rePosA1 = Vector3.Lerp(rePos1, rePos2, reT);

                        var rePos = Vector3.Lerp(rePosA0, rePosA1, reT);

                        var reNor = (rePos - rePosC).normalized;

                        positionGlobal = rePos;
                        normal = reNor;
                    }
                    else if (uv.y >= 1 - roundEdge)
                    {
                        var rePosC = Vector3.forward * (1 - roundEdge);

                        var rePos0 = rePosC + positionLocal;
                        var rePos1 = Vector3.forward + positionLocal;
                        var rePos2 = Vector3.forward;

                        var reT = Mathf.InverseLerp(1 - roundEdge, 1, uv.y);

                        var rePosA0 = Vector3.Lerp(rePos0, rePos1, reT);
                        var rePosA1 = Vector3.Lerp(rePos1, rePos2, reT);

                        var rePos = Vector3.Lerp(rePosA0, rePosA1, reT);

                        var reNor = (rePos - rePosC).normalized;

                        positionGlobal = rePos;
                        normal = reNor;
                    }
                }

                if (f < fragments)
                {
                    var offset = verticesCount2D * f;
                    var p1 = mesh2D.Lines[s * 2] + offset;
                    var p2 = mesh2D.Lines[s * 2 + 1] + offset;
                    var p3 = p1 + verticesCount2D;
                    var p4 = p2 + verticesCount2D;

                    mesh.Triangles.Add(p1);
                    mesh.Triangles.Add(p2);
                    mesh.Triangles.Add(p3);

                    mesh.Triangles.Add(p2);
                    mesh.Triangles.Add(p4);
                    mesh.Triangles.Add(p3);
                }

                mesh.Vertices.Add(positionGlobal);
                mesh.Uvs.Add(uv);
                mesh.Normals.Add(normal);
            }
        }

        return mesh;

    }

    public static void RemoveVertices(Mesh3D mesh, int count)
    {
        if (count <= 0)
        {
            return;
        }

        mesh.Vertices.RemoveRange(0, count);
        mesh.Normals.RemoveRange(0, count);
        mesh.Uvs.RemoveRange(0, count);
        mesh.Triangles.RemoveRange(0, count * 3);

        for (int j = 0; j < mesh.Triangles.Count; j++ )
        {
            mesh.Triangles[j] -= count;
        }
    }

    public static void GeneratePlane(float width, float height, int cols, int rows, ref Mesh3D mesh)
    {
        mesh = mesh ?? new Mesh3D();
        mesh.Clear();

        var positionOffset = new Vector3( - width / 2, 0, - height / 2 );
        var cellSize = new Vector3(width / cols, 0, height / rows);
        var normal = Vector3.up;

        for (int j = 0; j <= rows; j++)
        {
            for (int i = 0; i <= cols; i++)
            {
                var position = positionOffset + new Vector3(cellSize.x * i, 0, cellSize.z * j);
                var uv = new Vector2(i / (float)cols, j / (float)rows);

                mesh.Vertices.Add(position);
                mesh.Normals.Add(normal);
                mesh.Uvs.Add(uv);

                if (i < cols && j < rows)
                {
                    var index1 = (cols + 1) * j + i;
                    var index2 = index1 + 1;
                    var index3 = index1 + cols + 1;
                    var index4 = index3 + 1;

                    mesh.Triangles.Add( index1 );
                    mesh.Triangles.Add( index4 );
                    mesh.Triangles.Add( index2 );

                    mesh.Triangles.Add( index1 );
                    mesh.Triangles.Add( index3 );
                    mesh.Triangles.Add( index4 );
                }
            }
        }
    }

    public static void CreateBezierBendObject(Mesh3D mesh, Mesh2D mesh2D, BezierData bezier, int fragments)
    {
        mesh.Clear();

        var verticesCountStart = mesh.Vertices.Count;
        var verticesCount2D = mesh2D.Vertices.Count;

        for (int f = 0; f < fragments + 1; f++)
        {
            var t = Mathf.InverseLerp(0f, fragments, f);

            var bPoint = bezier.Lerp(t);

            for (int s = 0; s < verticesCount2D; s++)
            {
                var positionGlobal = bPoint.Rotation * mesh2D.Vertices[s] + bPoint.Position;
                var rotation = bPoint.Rotation;
                var normal = rotation * mesh2D.Normals[s];
                var uv = new Vector2(mesh2D.Us[s], t);

                if (f < fragments)
                {
                    var offset = verticesCount2D * f + verticesCountStart;
                    var p1 = mesh2D.Lines[s * 2] + offset;
                    var p2 = mesh2D.Lines[s * 2 + 1] + offset;
                    var p3 = p1 + verticesCount2D;
                    var p4 = p2 + verticesCount2D;

                    mesh.Triangles.Add(p1);
                    mesh.Triangles.Add(p2);
                    mesh.Triangles.Add(p3);

                    mesh.Triangles.Add(p2);
                    mesh.Triangles.Add(p4);
                    mesh.Triangles.Add(p3);
                }

                mesh.Vertices.Add(positionGlobal);
                mesh.Uvs.Add(uv);
                mesh.Normals.Add(normal);
            }
        }
    }

    public static void GenetareMesh3D(Mesh3D mesh3D, Mesh2D mesh2D, int fragments)
    {
        mesh3D.Clear();

        var positionOffset = Vector3.forward / fragments;
        var positionCenter = Vector3.zero;
        var verticesCount2D = mesh2D.Vertices.Count;
        var lineIndexOffset = 0;

        for (int f = 0; f <= fragments; f++)
        {
            var t = (float)f / fragments;

            for (int v = 0; v < mesh2D.Vertices.Count; v++)
            {
                var position = (Vector3)mesh2D.Vertices[v] + positionCenter;
                var normal = (Vector3)mesh2D.Normals[v];
                var uv = new Vector2(mesh2D.Us[v], t);

                mesh3D.Vertices.Add(position);
                mesh3D.Normals.Add(normal);
                mesh3D.Uvs.Add(uv);
            }

            if (f < fragments)
            {
                for (int l = 0; l < mesh2D.Lines.Count - 1; l += 2)
                {
                    var p1 = mesh2D.Lines[l] + lineIndexOffset;
                    var p2 = mesh2D.Lines[l + 1] + lineIndexOffset;
                    var p3 = p1 + verticesCount2D;
                    var p4 = p2 + verticesCount2D;

                    mesh3D.Triangles.Add(p1);
                    mesh3D.Triangles.Add(p2);
                    mesh3D.Triangles.Add(p3);

                    mesh3D.Triangles.Add(p2);
                    mesh3D.Triangles.Add(p4);
                    mesh3D.Triangles.Add(p3);
                }

                lineIndexOffset += verticesCount2D;
            }

            positionCenter += positionOffset;
        }
    }
}
