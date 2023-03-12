using UnityEngine;

public class MoveInTunnelState : MonoBehaviour
{
    public TunnelSplineModel Spline;
    public SplinePositionModel Position;
    public Vector3 Speed;

    private void Update()
    {
        Position.Data = Spline.MovePosition(Position.Data, Speed * Time.deltaTime);
        var worldPosition = Spline.GetWorldPositionRotation(Position.Data);

        transform.position = worldPosition.Position;
        transform.rotation = worldPosition.Rotation;
    }
}
