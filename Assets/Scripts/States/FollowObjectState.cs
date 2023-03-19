using UnityEngine;

public class FollowObjectState : MonoBehaviour
{
    public TunnelSplineModel Spline;
    public SplinePositionModel Position;
    public SplinePositionModel Target;

    public float Offset = 0.3f;

    private void Update()
    {
        var newData = new SplinePositionData { Position = new Vector3(Position.Data.Position.x, Position.Data.Position.y, Target.Data.Position.z) };
        Position.Data = Spline.MovePosition(newData, new Vector3(0,0,-Offset));
        var worldPosition = Spline.GetWorldPositionRotation(Position.Data);

        transform.position = worldPosition.Position;
        transform.rotation = worldPosition.Rotation;
    }
}
