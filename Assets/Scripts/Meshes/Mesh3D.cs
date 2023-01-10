using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mesh3D
{
    public List<Vector3> Vertices = new List<Vector3>();
    public List<Vector3> Normals = new List<Vector3>();
    public List<Vector2> Uvs = new List<Vector2>();
    public List<int> Triangles = new List<int>();

    public void Clear()
    {
        Vertices.Clear();
        Normals.Clear();
        Uvs.Clear();
        Triangles.Clear();
    }
}

public static class Mesh3DExtentions
{
    public static void FillMeshFilter(this MeshFilter filter, Mesh3D mesh3D, ref Mesh mesh)
    {
        mesh = mesh ?? new Mesh();
        filter.mesh = mesh;

        mesh.Clear();
        mesh.SetVertices(mesh3D.Vertices);
        mesh.SetTriangles(mesh3D.Triangles, 0);
        mesh.SetNormals(mesh3D.Normals);
        mesh.SetUVs(0, mesh3D.Uvs);
    }
}
