using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TunnelObjectMoveState : MonoBehaviour
{
    [SerializeField] private TunnelMotionData _data;
    [SerializeField] private BezierObject _bezier;
    [SerializeField] private TunnelMap _map;
    [SerializeField] private float _tStart = 1;
    [SerializeField] private float _speed = 0;
    [SerializeField] private Vector2 _localPosition;

    private float _t;

    private void OnEnable()
    {
        _t = _tStart;
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            var length = _map.DetailLength;
            var deltaDT = Time.deltaTime * _data.Speed;
            var deltaDO = Time.deltaTime * _speed;
            _t -= deltaDT / length;
            _t += deltaDO / length;
            _t = _t < 0 ? 1 + (_t % 1) : _t;
            _t = _t > 1 ? _t % 1 : _t;

            _t = Mathf.Clamp01(_t);
        }
        else
        {
            _t = _tStart;
        }

        var position = _bezier.Bezier.Lerp3(_t);

        transform.position = position.Position + 
            position.Rotation * (Vector3.up * _localPosition.y) +
            position.Rotation * (Vector3.left * _localPosition.x);
        transform.rotation = position.Rotation;
    }
}
