using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private float _speed = 5;

    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;
        }
    }
}
