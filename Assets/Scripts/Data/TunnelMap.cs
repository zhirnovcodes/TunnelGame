using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelMap : MonoBehaviour
{
    [SerializeField] private float _detailLength = 5;
    [SerializeField] private Vector3 _directionOne = Vector3.forward;
    [SerializeField] private Vector3 _directionTwo = Vector3.forward;

    public float DetailLength => _detailLength;
    public Vector3 DirectionOne 
    {
        get => _directionOne;

        set
        {
            _directionOne = value;
        }
    }

    public Vector3 DirectionTwo
    {
        get => _directionTwo;

        set
        {
            _directionTwo = value;
        }
    }
}
