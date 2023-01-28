using UnityEngine;

public struct TunnelDetailData
{
    public BezierData Bezier;
    public float Length;

    public static TunnelDetailData operator *(TunnelDetailData tdd, float value)
    {
        tdd.Scale(value);
        return tdd;
    }

    public void Scale(float value)
    {
        Bezier.P0 *= value;
        Bezier.P1 *= value;
        Bezier.P2 *= value;
        Bezier.P3 *= value;

        Length *= value;
    }
}
