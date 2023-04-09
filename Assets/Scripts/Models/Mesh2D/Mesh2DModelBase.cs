using UnityEngine;

public abstract class Mesh2DModelBase : MonoBehaviour
{
    public bool ShouldDraw;

    public abstract Mesh2D BuildMesh2D();


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (ShouldDraw)
        {
            var mesh = BuildMesh2D();
            for (int i = 0; i < mesh.Lines.Count; i += 2)
            {
                var p0 = mesh.Vertices[mesh.Lines[i]];
                var p1 = mesh.Vertices[mesh.Lines[i + 1]];
                Gizmos.DrawLine((Vector3)p0 + transform.position, (Vector3)p1 + transform.position);
            }
        }
    }
#endif
}
