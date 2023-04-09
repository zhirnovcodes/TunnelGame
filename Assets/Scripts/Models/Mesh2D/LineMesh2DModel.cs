using UnityEngine;

public class LineMesh2DModel : Mesh2DModelBase
{
    public float Width = 1;
    [Range(2, 128)] public int Slices = 2;

    public override Mesh2D BuildMesh2D()
    {
        var mesh = new Mesh2D();
        Mesh2DFactory.CreateLine(mesh, Slices);
        mesh.Scale(Width);
        return mesh;
    }
}
