using UnityEngine;

public class TunnelDetailObjectBuilder
{
    private BezierDetailModel _detail;
    private BezierData? _bezierData;
    private PositionRotation _point;
    private float _lengthOffset;
    private float _width = 1f;
    private Mesh _mesh;

    public TunnelDetailObjectBuilder WithBezierData(BezierData data)
    {
        _bezierData = data;
        return this;
    }

    public TunnelDetailObjectBuilder WithMesh(Mesh mesh)
    {
        _mesh = mesh;
        return this;
    }

    public TunnelDetailObjectBuilder WithBezierDetailModel(BezierDetailModel model)
    {
        _detail = model;
        return this;
    }

    public TunnelDetailObjectBuilder WithWorldPositionRotation(PositionRotation point)
    {
        _point = point;
        return this;
    }

    public TunnelDetailObjectBuilder WithLengthOffset(float length)
    {
        _lengthOffset = length;
        return this;
    }

    public TunnelDetailObjectBuilder WithWidth(float width)
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
