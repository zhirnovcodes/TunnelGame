using UnityEngine;

public class TunnelDetailObjectBuilder
{
    private BezierDetailModel _detail;
    private BezierData? _bezierData;
    private PositionRotation _point;
    private float _lengthOffset;
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

    public BezierDetailModel Build()
    {
        if (_mesh != null)
        {
            _detail.GetComponent<MeshFilter>().mesh = _mesh;
        }

        if (_bezierData.HasValue)
        {
            _detail.Data.Bezier = _bezierData.Value;
        }

        _detail.LengthOffset = _lengthOffset;

        _detail.Draw();

        _detail.transform.position = _point.Position;
        _detail.transform.rotation = _point.Rotation;

        return _detail;
    }
}
