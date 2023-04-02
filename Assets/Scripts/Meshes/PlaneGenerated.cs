using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PlaneGenerated : MonoBehaviour
{
    [SerializeField] private MeshFilter _filter;

    [SerializeField] private Vector2 _size;
    [SerializeField] private int _colls;
    [SerializeField] private int _rows;

    private Mesh3D _plane;
    private Mesh _mesh;

    private void OnValidate()
    {
        if (_filter == null)
        {
            _filter = GetComponent<MeshFilter>();
        }
        Mesh3DFactory.GeneratePlane(_size.x, _size.y, _colls, _rows, ref _plane);
        _mesh = _plane.ToMesh(_mesh);
        _filter.mesh = _mesh;
    }

}
