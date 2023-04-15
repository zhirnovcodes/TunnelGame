using UnityEngine;

public class BezierTest : MonoBehaviour
{
    public float Radius = 1;
    public float AngDegrees = 45f;
    public Vector2 Direction = Vector2.left;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        var angleRadians = AngDegrees * Mathf.Deg2Rad;
        var angleDegrees = AngDegrees;
        var startPos = Vector3.zero;
        var radiusV = (Vector3)Direction.normalized * Radius;
        var centralPos = startPos + radiusV;
        var up = Vector3.Cross(Vector3.forward, radiusV).normalized;

        var rotation1 = Quaternion.AngleAxis(angleDegrees / 2f, up);
        var rotation2 = Quaternion.AngleAxis(angleDegrees, up);

        var radius0 = -radiusV;
        var radius1 = (rotation1 * -radiusV).normalized * Radius / Mathf.Cos(angleRadians / 2f);
        var radius2 = rotation2 * -radiusV;

        var p0 = centralPos + radius0;
        var p1 = centralPos + radius1;
        var p2 = centralPos + radius2;

        var bezier = new BezierData(p0, p1, p2);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(new Ray(startPos, up));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, centralPos);
        Gizmos.DrawSphere(centralPos, 0.2f);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(p0, 0.2f);
        Gizmos.DrawSphere(p1, 0.2f);
        Gizmos.DrawSphere(p2, 0.2f);

        BezierExtentions.DrawBezierGizmos(bezier);
    }
#endif
}
