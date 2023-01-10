using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelMoveInternalState : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private BezierObject _bezier;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private TunnelMap _map;

    private MaterialPropertyBlock _propertyBlock;
    private int? _index;
    private float _t;
    private ITunnelMapGeneratorStrategy _strategy;

    private void OnEnable()
    {
        _t = 0;
        _strategy = _strategy ?? new TunnelMapGeneratorStrategy();
    }

    void Update()
    {
        var speedT = _playerData.Speed * Time.deltaTime / _map.DetailLength / 2f;
        _t += speedT;

        if (_t >= 1)
        {
            var d1 = _map.DirectionOne;
            var d2 = _map.DirectionTwo;
            _strategy.Generate(ref d1, ref d2);
            _map.DirectionOne = d1;
            _map.DirectionTwo = d2;

            _t = _t % 1;
        }

        var direction = Vector3.Slerp(_map.DirectionOne, _map.DirectionTwo, _t);

        var bezier = _bezier.Bezier;
        bezier.Point1.position = bezier.Point0.position + Vector3.forward * _map.DetailLength;
        bezier.Point2.position = bezier.Point1.position + direction * _map.DetailLength;



        var mainTexST = _renderer.sharedMaterial.GetVector("_MainTex_ST");
        var yScale = mainTexST.y;
        var yTile = mainTexST.w;
        var speedTile = _playerData.Speed * Time.deltaTime / _map.DetailLength * yScale;

        yTile += speedTile;
        mainTexST.w = yTile;

        //_propertyBlock = _propertyBlock ?? new MaterialPropertyBlock();
        //_propertyBlock.SetVector("_MainTex_ST", mainTexST);
        _renderer.sharedMaterial.SetVector("_MainTex_ST", mainTexST);
        Debug.Log(speedTile + " " + yTile);


        _bezier.Bezier.SendBezierToShader(_renderer, ref _propertyBlock, ref _index);

    }

    private void ResetBezier()
    {
    }
}
