using System;
using System.Collections.Generic;
using UnityEngine;

public struct BezierPoint
{
    public Vector3 Position;
    public Quaternion Rotation;
}

[System.Serializable]
public struct Bezier
{
    public Transform Point0;
    public Transform Point1;
    public Transform Point2;
    public Transform Point3;
}
