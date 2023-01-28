using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPosition : MonoBehaviour
{
    [SerializeField] private Vector3 _p0;
    [SerializeField] private Vector3 _p1;
    [SerializeField] private float _speed = 0.1f;

    private float _t = 0;

    private void OnEnable()
    {
        _t = 0;
    }

    void Update()
    {
        _t += _speed * Time.deltaTime;
        _t %= 2;

        var pos = _p0;
        if (_t <= 1)
        {
            pos = Vector3.Lerp(_p0, _p1, _t);
        }
        else
        {
            pos = Vector3.Lerp(_p1, _p0, _t % 1);
        }

        transform.position = pos;
    }
}
