using System.Collections.Generic;
using UnityEngine;

public class BezierObject : MonoBehaviour
{
    public Bezier Bezier;

    [SerializeField] private float _t;
    [SerializeField] private bool _shouldDraw;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_shouldDraw)
        {
            BezierExtentions.DrawBezier(Bezier, true, _t);
        }
    }

#endif

    // todo
    /*
    private void SetupBounds()
    {
        var bounds = _renderer.bounds;
        bounds.extents *= 10;
        _renderer.bounds = bounds;
    }
    

    */
}
