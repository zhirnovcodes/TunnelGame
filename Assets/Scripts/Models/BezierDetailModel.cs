using UnityEngine;

public class BezierDetailModel : MonoBehaviour
{
    public BezierDelailData Data;

    public event System.Action DrawInvoked;

    public void Draw()
    {
        DrawInvoked?.Invoke();
    }

    public float LengthOffset
    {
        set
        {

        }
    }
}
