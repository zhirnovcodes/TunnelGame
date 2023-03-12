using UnityEngine;

public class UpDownMapStrategy : IMapStrategy, IScaleable
{
    public float Scale { set; get; } = 1;

    private BezierData[] data;

    public UpDownMapStrategy()
    {
        var straight = BezierFactory.BuildStraight();
        var curvedUp = BezierFactory.BuildCurved90(Vector2.up);
        var curvedDown = BezierFactory.BuildCurved90(Vector2.down);

        data = new BezierData[] { straight, curvedUp, straight, curvedDown };

    }

    public BezierData GetBezier(int index)
    {
        return data[index % data.Length];
    }

}
