using System;
using UnityEngine;

public class BezierDetailModel : MonoBehaviour
{
    public event Action DrawInvoked;

    private BezierDelailData Data;

    public Vector2 TextureOffset 
    { 
        set 
        { 
            Data.TextureOffset = value; 
        }
    }

    public void Draw()
    {
        DrawInvoked?.Invoke();
    }
}
