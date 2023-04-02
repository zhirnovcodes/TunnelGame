using UnityEngine;

public class StraightMap : MapBase
{
    public override ITunnelMap Map { get; } = new StraightTunnelMap();

}
