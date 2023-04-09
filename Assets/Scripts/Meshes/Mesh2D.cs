using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mesh2D
{
    public List<Vector2> Vertices = new List<Vector2>();
    public List<Vector2> Normals = new List<Vector2>();
    public List<float> Us = new List<float>();
    public List<int> Lines = new List<int>();

    public void Clear()
    {
        Vertices.Clear();
        Normals.Clear();
        Us.Clear();
        Lines.Clear();
    }

    public void RotateClockWise(float angRad)
    {
        for (int i = 0; i < Vertices.Count; i++)
        {
            Vertices[i] = Vertices[i].RotateClockWise(angRad);
            Normals[i] = Normals[i].RotateClockWise(angRad);
        }
    }

    public void Scale(float value)
    {
        for (int i = 0; i < Vertices.Count; i++)
        {
            Vertices[i] *= value;
        }
    }

    public float CalculateWidth()
    {
        var sum = 0f;

        for (int i = 0; i < Lines.Count; i+=2)
        {
            var p0 = Vertices[Lines[i]];
            var p1 = Vertices[Lines[i + 1]];

            sum += (p0 - p1).magnitude;
        }

        return sum;
    }
}
