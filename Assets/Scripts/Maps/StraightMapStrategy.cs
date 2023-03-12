using UnityEngine;

public class StraightMapStrategy : IMapStrategy
{
    public float DetailLength { get; set; } = 1;

    public BezierData GetBezier(int index)
    {
        var data = new BezierData
        {
            P1 = Vector3.forward * DetailLength / 2f,
            P2 = Vector3.forward * DetailLength,
            Length = DetailLength
        };

        return data;
    }
}
