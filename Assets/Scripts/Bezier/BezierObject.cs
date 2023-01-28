using UnityEngine;

public class BezierObject : MonoBehaviour
{
    public Bezier Bezier;

    [SerializeField] private float _t;
    [SerializeField] private bool _shouldDraw;

    public void SetData(BezierData data)
    {
        Bezier.GetTransform(0).position = data.P0;
        Bezier.GetTransform(1).position = data.P1;
        Bezier.GetTransform(2).position = data.P2;
        if (data.PointsCount > 3)
        {
            Bezier.GetTransform(3).position = data.P3;
        }
    }

    public BezierData ToData()
    {
        var count = Bezier.GetTransform(3) == null ? 3 : 4;
        return new BezierData
        {
            PointsCount = count,
            P0 = Bezier.Point0.position,
            P1 = Bezier.Point1.position,
            P2 = Bezier.Point2.position,
            P3 = count == 3 ? Vector3.zero : Bezier.Point3.position,
        };
    }

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
