using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TunnelWiggleState : MonoBehaviour
{
    [SerializeField] private TunnelMotionData _motionData;
    [SerializeField] private BezierObject _bezierObject;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private MapBase _map;
    [SerializeField] private MeshGenerator _meshData;
    [SerializeField] private TunnelViewController _view;
    [SerializeField] private float _startPosition;
    [SerializeField] private float _xDivY = 1;
    [SerializeField] private bool _isTextureMirrored;

    private void OnEnable()
    {
        _motionData.StartPosition = _startPosition;
        _motionData.LengthPassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var speed = _motionData.Speed;

        if (!Application.isPlaying)
        {
            speed = 0;
        }

        var t = _motionData.StartPosition;
        var t0 = Mathf.FloorToInt(t);
        var detail = _map.Strategy.GetBezier(t0);
        var length = detail.Length;
        var deltaDist = speed * Time.deltaTime;
        var deltaT = deltaDist / length;
        t += deltaT;

        var detail0 = _map.Strategy.GetBezier(Mathf.FloorToInt(t));
        var detail1 = _map.Strategy.GetBezier(Mathf.FloorToInt(t) + 1);

        //t %= 1;

        _motionData.StartPosition = t;
        _motionData.LengthPassed += deltaDist;

        var bezier = detail0.SlerpToBezier(detail1, t % 1);
        _view.Bezier = bezier;
        //_bezierObject.SetData(bezier);


        var texOffset = _view.TextureOffset;
        var radius = _meshData.Radius;
        var perimeter = 2 * Mathf.PI * radius;
        var xTexScale = texOffset.x;
        var xTexLength = perimeter / xTexScale;
        var yTexLength = xTexLength / _xDivY;
        var yDetLength = detail0.Length;
        var yTexScale = yDetLength / yTexLength;
        var xTexOffset = texOffset.z;
        var lengthStart = _motionData.LengthPassed;
        var yTexOffset = lengthStart / yTexLength % 2;
        var texOffsetNew = new Vector4(xTexScale, yTexScale, xTexOffset, yTexOffset);
        _view.TextureOffset = texOffsetNew;

        //_view.SendToShader();
    }
}
