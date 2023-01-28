public class TunnelDetailBuilder
{
    private ICalculateLengthStrategy _lengthStrategy;

    private BezierData _bezier;

    private float _length;

    public TunnelDetailBuilder WithLengthStrategy(ICalculateLengthStrategy lengthStrategy)
    {
        _lengthStrategy = lengthStrategy;
        return this;
    }

    public TunnelDetailBuilder WithLength(float length)
    {
        _length = length;
        return this;
    }

    public TunnelDetailBuilder WithBezierObject(BezierObject bezier)
    {
        _bezier = bezier.ToBezierData();
        return this;
    }

    public TunnelDetailBuilder WithBezierData(BezierData bezier)
    {
        _bezier = bezier;
        return this;
    }

    public TunnelDetailData Build()
    {
        var length = _lengthStrategy == null ? _length : _lengthStrategy.Length(_bezier);
        return new TunnelDetailData { Bezier = _bezier, Length = length };
    }
}
