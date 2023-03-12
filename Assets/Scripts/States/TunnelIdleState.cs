using UnityEngine;

[ExecuteInEditMode]
public class TunnelIdleState : MonoBehaviour
{
    [SerializeField] private BezierObject _bezier;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private MapBase _map;
    [SerializeField] private MonoBehaviour _nextState;

    private MaterialPropertyBlock _propertyBlock;
    private int? _index;

    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            ResetBezier();
            if (_nextState == null)
            {
                return;
            }
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
#endif
    }

    private void ResetBezier()
    {
        if (_renderer == null || _bezier == null || _map == null)
        {
            return;
        }

        var detail = _map.Strategy.GetBezier(0);
        var bezier = _bezier;

        bezier.P0.position = detail.P0;
        bezier.P1.position = detail.P1;
        bezier.P2.position = detail.P2;
        bezier.P3.position = detail.P3;
        
        BezierExtentions.SendBezierToShader( bezier.ToBezierData(), _renderer, ref _propertyBlock, ref _index );
    }
}
