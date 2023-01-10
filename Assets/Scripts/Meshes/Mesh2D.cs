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
}
