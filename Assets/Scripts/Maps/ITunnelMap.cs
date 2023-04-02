public interface ITunnelMap
{
    int GetBezierIndex(int index);
    
    BezierData GetBezier(int bezierIndex);

    int GetBezierCount();
}

public interface IScaleable
{
    float Scale { get; set; }
}