using UnityEngine;

public class UpDownMap : MapBase
{
    private ITunnelMap _strategy;

    public override ITunnelMap Map { get; } = new UpDownTunnelMap();
}
