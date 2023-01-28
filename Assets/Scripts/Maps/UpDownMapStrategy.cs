using UnityEngine;

public class UpDownMapStrategy : IMapStrategy, IScaleable
{
    public float Scale { set; get; } = 1;

    private TunnelDetailData[] data;

    public UpDownMapStrategy()
    {
        var straight = TunnelDetailFactory.BuildStraight();
        var curvedUp = TunnelDetailFactory.BuildCurved(Vector2.up);
        var curvedDown = TunnelDetailFactory.BuildCurved(Vector2.down);

        data = new TunnelDetailData[] { straight, curvedUp, straight, curvedDown };

    }

    public TunnelDetailData GetDetail(int index)
    {
        return data[index % data.Length] * Scale;
    }

}
