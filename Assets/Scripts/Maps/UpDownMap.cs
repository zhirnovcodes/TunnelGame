using UnityEngine;

public class UpDownMap : MapBase
{
    private ISplineMap _strategy;

    public override ISplineMap Map { get; } = new UpDownTunnelMap();
}
