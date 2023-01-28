using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelMotionData : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _startPosition;
    [SerializeField] private float _lengthPassed;

    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;
        }
    }

    public float StartPosition
    {
        get => _startPosition;
        set
        {
            _startPosition = value;
        }
    }

    public float LengthPassed
    {
        get => _lengthPassed;
        set
        {
            _lengthPassed = value;
        }
    }
}
