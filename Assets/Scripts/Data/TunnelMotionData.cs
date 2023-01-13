using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelMotionData : MonoBehaviour
{
    [SerializeField] private float _speed;

    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;
        }
    }
}
