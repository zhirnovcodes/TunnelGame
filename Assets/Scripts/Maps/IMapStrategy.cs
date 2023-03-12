public interface IMapStrategy
{
    BezierData GetBezier(int index);
}

public interface IScaleable
{
    float Scale { get; set; }
}