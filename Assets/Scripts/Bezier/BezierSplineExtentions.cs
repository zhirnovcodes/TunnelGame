using UnityEngine;

public static class BezierSplineExtentions
{
    public static PositionRotation Lerp(this BezierSplineData spline, float t)
    {
        var count = spline.BezierList?.Count ?? 0;
        var currentIndex = Mathf.Max(0, Mathf.FloorToInt(t));

        if (count == 0)
        {
            return new PositionRotation();
        }

        if (currentIndex >= count)
        {
            currentIndex = count - 1;
            t = 1;
        }
        else
        {
            t %= 1;
        }

        var bezier = spline.BezierList[currentIndex];
        return bezier.Lerp(t);
    }
}
