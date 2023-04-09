using System.Collections.Generic;
using UnityEngine;

public class SplineParametrizationMap
{

    private readonly Dictionary<int, float> Map = new Dictionary<int, float>();

    public float ArcLength { get; private set; }

    public int IndexMin { get; private set; } = 0;
    public int IndexMax { get; private set; } = -1;

    public float LengthMin => IndexMin * ArcLength;
    public float LengthMax => IndexMax * ArcLength;

    public float TMin => Map[IndexMin];
    public float TMax => Map[IndexMax];

    public SplineParametrizationMap(float arcLength)
    {
        ArcLength = arcLength;
    }

    public float GetTFromLength(float length)
    {
        if (Map.Count == 0)
        {
            return 0;
        }

        var index0 = Mathf.Clamp(Mathf.FloorToInt(length / ArcLength), IndexMin, IndexMax);
        var index1 = Mathf.Clamp(index0 + 1, IndexMin, IndexMax);

        var t0 = Map[index0];
        var t1 = Map[index1];

        var l0 = index0 * ArcLength;
        var l1 = index1 * ArcLength;

        return Mathf.Lerp(t0, t1, Mathf.InverseLerp(l0, l1, length));
    }

    public void Append(float t)
    {
        IndexMax++;

        Map.Add(IndexMax, t);
    }

    public float Peek()
    {
        if (Map.Count == 0)
        {
            return -1;
        }

        return Map[IndexMin];
    }

    public bool Pop()
    {
        if (Map.Count == 0)
        {
            return false;
        }
        Map.Remove(IndexMin);
        IndexMin++;
        return Map.Count > 0;
    }

    public void Clear(float arcLength)
    {
        Clear();
        ArcLength = arcLength;
    }

    public void Clear()
    {
        Map.Clear();
        IndexMin = 0;
        IndexMax = -1;
    }
}
