using System.Collections.Generic;

[System.Serializable]
public class BezierSplineData
{
    public List<BezierData> BezierList = new List<BezierData>();
    public float Length;
}
