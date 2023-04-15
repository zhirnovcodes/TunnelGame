using UnityEngine;

public class FollowObjectState : MonoBehaviour
{
    public TunnelSplineModel Spline;
    public SplinePositionModel Position;
    public SplinePositionModel Target;

    public Vector3 Offset = new Vector3(0,0,-1f);
    public bool lockRotation = false;

    private void Update()
    {
        var targetPosition = Target?.Position ?? Vector3.zero;
        var newPosition = targetPosition + Offset;

        Position.Position = newPosition;

        var worldPosition = Spline.ToWorldSpace(newPosition);


        transform.position = worldPosition.Position;

        if (!lockRotation)
        {
            transform.rotation = worldPosition.Rotation;
        }
    }
}
