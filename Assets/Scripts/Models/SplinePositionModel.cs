using UnityEngine;

public class SplinePositionModel : MonoBehaviour
{
    [SerializeField] private SplinePositionData Data;

    public Vector3 Position 
    {
        get => Data.Position;
        set { Data.Position = value; }
    }
}
