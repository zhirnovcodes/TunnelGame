using UnityEngine;

public class StraightMap : MapBase
{
    public override ISplineMap Map { get; } = new StraightTunnelMap();

}
