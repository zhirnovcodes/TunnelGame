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
        var detail = _map.Strategy.GetDetail(0);
        var bezier = _bezier.Bezier;
        bezier.Point0.position = detail.Bezier.P0;
        bezier.Point1.position = detail.Bezier.P1;
        bezier.Point2.position = detail.Bezier.P2;
        if (bezier.Point3 != null)
        { bezier.Point3.position = detail.Bezier.P3; }
        //BezierExtentions.SendBezierToShader()

        //_bezier.Bezier.SendBezierToShader(_renderer, ref _propertyBlock, ref _index);
    }
}
