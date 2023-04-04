using UnityEngine;

public class UpDownTunnelMap : ISplineMap, IScaleable
{
    public float Scale { set; get; } = 1;

    private BezierData[] Data;

    public UpDownTunnelMap()
    {
        var straight = BezierFactory.BuildStraight();
        var curvedUp = BezierFactory.BuildCurved90(Vector2.up);
        var curvedDown = BezierFactory.BuildCurved90(Vector2.down);

        Data = new BezierData[] { straight, curvedUp, curvedDown, straight };

        foreach (var d in Data)
        {
            d.Scale(Scale);
        }
    }

    public BezierData GetBezier(int index)
    {
        return Data[index];
    }

	public int GetBezierIndex(int index)
	{
        return index % Data.Length;

    }

	public int GetBezierCount()
	{
        return Data.Length;
	}
}
