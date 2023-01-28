using System.Collections;
using UnityEngine;

[System.Serializable]
public struct BezierData
{
    public Vector3 P0;
    public Vector3 P1;
    public Vector3 P2;
    public Vector3 P3;

    public int PointsCount;
}


[System.Serializable]
public struct BezierData3
{
    public Vector3 P0;
    public Vector3 P1;
    public Vector3 P2;
}

[System.Serializable]
public struct BezierData4
{
    public Vector3 P0;
    public Vector3 P1;
    public Vector3 P2;
    public Vector3 P3;
}
