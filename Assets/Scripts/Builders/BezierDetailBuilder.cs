using UnityEngine;

public class BezierDetailBuilder
{
    private BezierDetailModel _detail;
    private BezierData? _bezierData;
    private PositionRotation _point;
    private float _lengthOffset;
    private float _width = 1f;
    private Mesh _mesh;

    public BezierDetailBuilder WithBezierData(BezierData data)
    {
        _bezierData = data;
        return this;
    }

    public BezierDetailBuilder WithMesh(Mesh mesh)
    {
        _mesh = mesh;
        return this;
    }

    public BezierDetailBuilder WithBezierDetailModel(BezierDetailModel model)
    {
        _detail = model;
        return this;
    }

    public BezierDetailBuilder WithWorldPositionRotation(PositionRotation point)
    {
        _point = point;
        return this;
    }

    public BezierDetailBuilder WithLengthOffset(float length)
    {
        _lengthOffset = length;
        return this;
    }

    public BezierDetailBuilder WithWidth(float width)
    {
        _width = width;
        return this;
    }

    public BezierDetailModel Build()
    {
        if (_mesh != null)
        {
            _detail.GetComponent<MeshFilter>().mesh = _mesh;
        }

        var length = _bezierData.HasValue ? _bezierData.Value.Length : 1f;

        _detail.Length = length;
        _detail.LengthOffset = _lengthOffset;
        _detail.Width = _width;

        _detail.Draw();

        _detail.transform.position = _point.Position;
        _detail.transform.rotation = _point.Rotation;

        return _detail;
    }
}
