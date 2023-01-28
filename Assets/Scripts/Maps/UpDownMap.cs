using UnityEngine;

public class UpDownMap : MapBase
{
    private IMapStrategy _strategy;

    public override IMapStrategy Strategy { get; } = new UpDownMapStrategy();
}
