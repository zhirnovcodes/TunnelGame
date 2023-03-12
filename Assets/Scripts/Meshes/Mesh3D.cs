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

    public void ToMesh(ref Mesh mesh)
    {
        mesh = mesh ?? new Mesh();

        mesh.Clear();
        mesh.SetVertices(Vertices);
        mesh.SetTriangles(Triangles, 0);
        mesh.SetNormals(Normals);
        mesh.SetUVs(0, Uvs);
    }
}
