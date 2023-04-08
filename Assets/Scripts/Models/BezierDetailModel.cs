using System;
using UnityEngine;

public class BezierDetailModel : MonoBehaviour
{
    public event Action<BezierDelailData> DrawInvoked;

    private BezierDelailData Data;

    public float Length 
    { 
        set 
        { 
            Data.Length = value;
        }
    }

    public float LengthOffset
    {
        set
        {
            Data.LengthOffset = value;
        }
    }

    public float Width
    {
        set
        {
            Data.Width = value;
        }
    }

    public void Draw()
    {
        DrawInvoked?.Invoke(Data);
    }
}
