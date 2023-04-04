using System.Collections.Generic;
using UnityEngine;

public class SplineParametrizationMap
{

    private readonly Dictionary<int, float> Map = new Dictionary<int, float>();

    public float ArcLength { get; private set; }

    public int IndexMin { get; private set; }
    public int IndexMax { get; private set; }

    public float LengthMin => IndexMin * ArcLength;
    public float LengthMax => IndexMax * ArcLength;

    public float TMin => Map[IndexMin];
    public float TMax => Map[IndexMax];

    public SplineParametrizationMap(float arcLength)
    {
        ArcLength = arcLength;
    }

    public float GetT(float length)
    {
        if (Map.Count == 0)
        {
            return 0;
        }

        length = Mathf.Clamp(length, LengthMin, LengthMax);

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

    public void Pop()
    {
        if (Map.Count == 0)
        {
            return;
        }
        Map.Remove(IndexMin);
        IndexMin++;
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
