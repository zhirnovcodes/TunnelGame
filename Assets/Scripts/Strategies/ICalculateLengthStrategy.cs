public interface ICalculateLengthStrategy
{
    float Length(BezierData data);
}

public class DeltaCalculateLengthStrategy : ICalculateLengthStrategy
{
    public int Precision { get; set; } = 1000;

    public float Length(BezierData data)
    {
        var s = 0f;
        var pBefore = data.Lerp(0);
        for (float i = 1f / Precision; i < 1; i += 1f / Precision)
        {
            var p = data.Lerp(i);
            s += (pBefore.Position - p.Position).magnitude;
            pBefore = p;
        }
        var p1 = data.Lerp(1);
        s += (pBefore.Position - p1.Position).magnitude;
        return s;
    }
}
