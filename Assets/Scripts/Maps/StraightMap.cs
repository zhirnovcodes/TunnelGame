using UnityEngine;

public class StraightMap : MapBase
{
    public override IMapStrategy Strategy { get; } = new StraightMapStrategy();

}
