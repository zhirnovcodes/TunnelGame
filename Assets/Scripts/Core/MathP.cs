using UnityEngine;

public static class MathP
{
    public const float TAU = Mathf.PI * 2;

    public static Vector2 RotateCounterClockWise(this Vector2 v, float rad)
    {
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
    public static Vector2 RotateClockWise(this Vector2 v, float rad)
    {
        return RotateCounterClockWise(v, -rad);
    }

    public static Vector3 ScaleVector(this Vector3 vector, Vector3 scale)
    {
        return new Vector3(vector.x * scale.x, vector.y * scale.y, vector.z * scale.z);
    }
}
