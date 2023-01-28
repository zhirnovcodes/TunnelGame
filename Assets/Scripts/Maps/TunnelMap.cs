using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    Straight,
    UpDown,
    AllSides
}

public class TunnelMap : MapBase
{
    [SerializeField] private MapType _type = MapType.Straight;
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

    private IMapStrategy _strategy;

    public override IMapStrategy Strategy
    {
        get
        {
            if (_strategy == null)
            {
                switch (_type)
                {
                    case MapType.Straight:
                        _strategy = new StraightMapStrategy();
                        break;
                    case MapType.AllSides:
                        _strategy = new AllSidesMapStrategy();
                        break;
                    case MapType.UpDown:
                        _strategy = new UpDownMapStrategy();
                        break;
                }
            }
            return _strategy;
        }
    }
}
