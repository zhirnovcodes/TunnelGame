using UnityEngine;

[ExecuteInEditMode]
public class TunnelIdleState : MonoBehaviour
{
    [SerializeField] private BezierObject _bezier;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private TunnelMap _map;
    [SerializeField] private TunnelMoveInternalState _nextState;

    private MaterialPropertyBlock _propertyBlock;
    private int? _index;

    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            ResetBezier();
            _nextState.enabled = true;
            this.enabled = false;
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            ResetBezier();
        }
        return;
#endif
    }

    private void ResetBezier()
    {
        if (_renderer == null || _bezier == null || _map == null)
        {
            return;
        }
        var bezier = _bezier.Bezier;
        bezier.Point1.position = bezier.Point0.position + Vector3.forward * _map.DetailLength;
        bezier.Point2.position = bezier.Point1.position + _map.DirectionTwo * _map.DetailLength;

        _bezier.Bezier.SendBezierToShader(_renderer, ref _propertyBlock, ref _index);
    }
}
