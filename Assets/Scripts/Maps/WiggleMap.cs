using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleMap : ISplineMap
{
    private List<BezierData> Bezier = new List<BezierData>();

    public WiggleMap(float radius, float angleRad, float scale = 1)
    {
        var direction = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right, Vector2.one, -Vector2.one, new Vector2(-1,1), new Vector2(1, -1) };

        var straight = BezierFactory.BuildStraight();

        Bezier.Add(straight);

        for (int i = 0; i < direction.Length; i++)
        {
            var wiggled = BezierFactory.BuildCurved(direction[i], angleRad, radius);

            wiggled.Scale(scale);
            Bezier.Add(wiggled);
        }
    }

    public BezierData GetBezier(int bezierIndex)
    {
        return Bezier[bezierIndex];
    }

    public int GetBezierCount()
    {
        return Bezier.Count;
    }

    public int GetBezierIndex(int index)
    {
        return index % Bezier.Count;
    }
}
