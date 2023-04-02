using UnityEngine;

public class StraightTunnelMap : ITunnelMap
{
	public float DetailLength { get; set; } = 1;

    public BezierData GetBezier(int bezierIndex)
    {
        var data = new BezierData ( Vector3.forward * DetailLength / 2f, Vector3.forward * DetailLength ) 
            { Length = DetailLength };

        return data;
    }

	public int GetBezierCount()
	{
		return 1;
	}

	public int GetBezierIndex(int index)
	{
		return 0;
	}
}
