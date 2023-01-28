public interface IMapStrategy
{
    TunnelDetailData GetDetail(int index);
}

public interface IScaleable
{
    float Scale { get; set; }
}