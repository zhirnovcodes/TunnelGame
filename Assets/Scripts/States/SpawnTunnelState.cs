using UnityEngine;

public class SpawnTunnelState : MonoBehaviour
{
    public SplinePositionModel Observer;
    public TunnelSplineModel Spline;
    public MapBase Map;
    public BezierDetailModel Prefab;
    public Mesh2DModelBase Mesh2D;

    public int MaxCount = 5;
    public float FragmentsLength = 0.5f;

    private SplineSpawnStrategy SpawnStrategy;
    private float LastObserverPosition;

    private void Start()
    {
        var mesh2D = Mesh2D.BuildMesh2D();

        var map = new RoadMap(1000, 0.2f, 1);

        SpawnStrategy = new SplineSpawnStrategy(mesh2D, Prefab, map, Spline, FragmentsLength, mesh2D.CalculateWidth());

        for (int i = 0; i < MaxCount; i++)
        {
            SpawnStrategy.Spawn();
        }
    }

    private void Update()
    {
        var position = Observer.Data.Position.z;

        if (position - LastObserverPosition >= 4)
        {
            LastObserverPosition = position;
            SpawnStrategy.Despawn();
            SpawnStrategy.Spawn();
        }
    }
}
