using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] [Range(0.01f,30)] private float _radius = 1;
    [SerializeField] [Range(0f,360)] private float _spinDegrees = 0;
    [SerializeField] [Range(3,32)] private int _slices = 3;
    [SerializeField] [Range(1,32)] private int _fragments = 1;
    [SerializeField] [Range(0.0f, 0.5f)] private float _roundEdge = 0.1f;
    [SerializeField] private bool _drawTriangles;
    [SerializeField] private bool _shadedInside;

    private Mesh _mesh;
    private Mesh2D _mesh2D;
    private Mesh3D _tunnel;

    public void RepaintTunnel()
    {
        ResetMesh3D();
        FillMeshFilter();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetMesh2D();
        ResetMesh3D();
        FillMeshFilter();
    }

    private void FillMeshFilter()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        _mesh.SetVertices(_tunnel.Vertices);
        _mesh.SetTriangles(_tunnel.Triangles, 0);
        _mesh.SetNormals(_tunnel.Normals);
        _mesh.SetUVs(0, _tunnel.Uvs);
    }

    private void DrawTunnel()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.white;

        for (int i = 0; i < _tunnel.Triangles.Count; i += 3)
        {
            var p1 = _tunnel.Triangles[i];
            var p2 = _tunnel.Triangles[i + 1] ;
            var p3 = _tunnel.Triangles[i + 2];


            UnityEditor.Handles.DrawLine(_tunnel.Vertices[p1], _tunnel.Vertices[p2]);
            UnityEditor.Handles.DrawLine(_tunnel.Vertices[p2], _tunnel.Vertices[p3]);
            UnityEditor.Handles.DrawLine(_tunnel.Vertices[p3], _tunnel.Vertices[p1]);
        }
#endif
    }

    private void ResetMesh2D()
    {
        if (_mesh2D == null)
        {
            _mesh2D = new Mesh2D();
        }
        Mesh2DFactory.FillCircleMesh2D(_mesh2D, _slices, _shadedInside, _radius, _spinDegrees);
    }

    private void ResetMesh3D()
    {
        _tunnel = Mesh3DFactory.CreateCapsule( _mesh2D, _fragments, _roundEdge );
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ResetMesh2D();
        ResetMesh3D();
        FillMeshFilter();
    }
#endif

    private void OnDrawGizmosSelected()
    {
        if (_drawTriangles)
        {
            DrawTunnel();
        }
    }
}
