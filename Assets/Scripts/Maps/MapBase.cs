using UnityEngine;

public abstract class MapBase : MonoBehaviour
{
    [SerializeField] private float _scale;

    public abstract IMapStrategy Strategy { get; }

    private void OnValidate()
    {
        if (Strategy is IScaleable scaleable)
        {
            scaleable.Scale = _scale;
        }
    }
}
