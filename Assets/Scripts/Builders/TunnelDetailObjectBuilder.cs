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

        _detail.TextureOffset = new Vector2( _lengthOffset, 0);

        _detail.Draw();

        _detail.transform.position = _point.Position;
        _detail.transform.rotation = _point.Rotation;

        return _detail;
    }
}
