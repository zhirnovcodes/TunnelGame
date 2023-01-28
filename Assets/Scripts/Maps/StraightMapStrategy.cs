using UnityEngine;

public class StraightMapStrategy : IMapStrategy
{
    private TunnelDetailBuilder _builder = new TunnelDetailBuilder();

    public float DetailLength { get; set; } = 1;

    public TunnelDetailData GetDetail(int index)
    {
        var data = new BezierData
        {
            P1 = Vector3.forward * DetailLength / 2f,
            P2 = Vector3.forward * DetailLength
        };
        return _builder.WithBezierData(data).WithLength(DetailLength).Build();
    }
}
