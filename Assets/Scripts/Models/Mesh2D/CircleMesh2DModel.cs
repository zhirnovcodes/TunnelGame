using UnityEngine;

public class CircleMesh2DModel : Mesh2DModelBase
{
    public float Radius = 1;
    [Range(3,128)] public int Slices = 3;
    public float SpinDegrees = 0;
    public bool IsStrictEdge;
    public bool IsShadedInside;

    public override Mesh2D BuildMesh2D()
    {
        Mesh2D mesh2D = new Mesh2D();

        if (IsStrictEdge)
        {
            Mesh2DFactory.CreateStrictEdge(mesh2D, Slices, IsShadedInside);
            mesh2D.Scale(Radius);
        }
        else
        {
            Mesh2DFactory.CreateCircleMesh2D(mesh2D, Slices, IsShadedInside, Radius);
        }

        mesh2D.RotateClockWise(SpinDegrees * Mathf.Deg2Rad);

        return mesh2D;
    }
}
