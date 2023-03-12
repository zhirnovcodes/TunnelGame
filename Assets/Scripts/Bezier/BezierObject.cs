using UnityEngine;

public class BezierObject : MonoBehaviour
{
    [SerializeField] private Transform _p0;
    [SerializeField] private Transform _p1;
    [SerializeField] private Transform _p2;
    [SerializeField] private Transform _p3;

    [SerializeField] private float _t;
    [SerializeField] private bool _shouldDraw;

    public Transform P0 => _p0;
    public Transform P1 => _p1;
    public Transform P2 => _p2;
    public Transform P3 => _p3;

    public BezierData ToBezierData()
    {
        return new BezierData
        {
            P0 = GetPosition(0),
            P1 = GetPosition(1),
            P2 = GetPosition(2),
            P3 = GetPosition(3),

            PointsCount = GetPointsCount()
        };
    }

    public int GetPointsCount()
    {
        if (_p0 == null)
        {
            return 0;
        }
        if (_p1 == null)
        {
            return 1;
        }
        if (_p2 == null)
        {
            return 2;
        }
        if (_p3 == null)
        {
            return 3;
        }

        return 4;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_shouldDraw)
        {
            DrawBezier(true, _t);
        }
    }

    private void DrawBezier(bool shouldDrawHandles = true, float t = 0)
    {
        var count = GetPointsCount();

        switch (count)
        {
            case 0: return;
            case 1: return;
            case 2: UnityEditor.Handles.DrawLine(GetPosition(0), GetPosition(1)); break;
            case 3:
                UnityEditor.Handles.DrawBezier(
                    GetPosition(0),
                    GetPosition(1),
                    GetPosition(2),
                    GetPosition(2),
                    Color.white, Texture2D.whiteTexture, 2);
                break;
            case 4:
                UnityEditor.Handles.DrawBezier(
                    GetPosition(0),
                    GetPosition(1),
                    GetPosition(2),
                    GetPosition(3),
                    Color.white, Texture2D.whiteTexture, 2);
                break;
        }
#endif
    }

    private Vector3 GetPosition(int index)
    {
        Vector3? position = null;

        switch (index)
        {
            case 0: position = _p0?.position; break;
            case 1: position = _p1?.position; break;
            case 2: position = _p2?.position; break;
            case 3: position = _p3?.position; break;
        }

        return position ?? Vector3.zero;
    }

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
