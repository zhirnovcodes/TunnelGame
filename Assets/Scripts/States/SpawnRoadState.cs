using UnityEngine;

public class SpawnRoadState : MonoBehaviour
{
    public SplinePositionModel Observer;
    public TunnelSplineModel Spline;
    public BezierDetailModel Prefab;
    public int MaxCount = 5;

    public int Slices = 3;
    public float FragmentsLength = 0.2f;
    public float Width = 5;
    public float Height = 0.5f;
    public float Scale = 5;
    public float SpinDegrees = 0;

    private SplineSpawnStrategy SpawnStrategy;

    private void Start()
    {
        Mesh2D mesh2D = new Mesh2D();
        Mesh2DFactory.CreateLine(mesh2D, Slices);
        mesh2D.RotateClockWise(SpinDegrees * Mathf.Deg2Rad);
        mesh2D.Scale(Width);

        var map = new RoadMap(1000, Height, Scale);

        SpawnStrategy = new SplineSpawnStrategy(mesh2D, Prefab, map, Spline, FragmentsLength, Width, false);

        for (int i = 0; i < MaxCount; i++)
        {
            SpawnStrategy.Spawn();
        }
    }

    private void Update()
    {
        var position = Observer.Position.z;
        var minPosition = Spline.LengthOffset;

        if (position - minPosition >= Scale * 3)
        {
            SpawnStrategy.Despawn();
            SpawnStrategy.Spawn();
        }
    }
}
