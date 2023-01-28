using UnityEngine;

public class TunnelDetailObjectBuilder
{
    private BezierData _data;
    private Transform _transform;
    private BezierPoint _point;
    private BezierObject _bezierObject;
    public TunnelDetailObjectBuilder WithBezierData(BezierData data)
    {
        _data = data;
        return this;
    }

    public TunnelDetailObjectBuilder WithTransform(Transform transform)
    {
        _transform = transform;
        return this;
    }

    public TunnelDetailObjectBuilder WithBezierPoint(BezierPoint point)
    {
        _point = point;
        return this;
    }

    public TunnelDetailObjectBuilder WithBezierObject(BezierObject bO)
    {
        _bezierObject = bO;
        return this;
    }

    public Transform Build()
    {
        _transform.position = _point.Position;
        _transform.rotation = _point.Rotation;

        _bezierObject.Bezier.Point0.localPosition = _data.P0;
        _bezierObject.Bezier.Point1.localPosition = _data.P1;
        _bezierObject.Bezier.Point2.localPosition = _data.P2;
        if (_bezierObject.Bezier.Point3 != null)
        {
            _bezierObject.Bezier.Point3.localPosition = _data.P3;
        }

        return _transform;
    }
}
