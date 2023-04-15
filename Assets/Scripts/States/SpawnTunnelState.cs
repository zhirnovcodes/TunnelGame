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
    public float OffetToSpawn = 8;

    private SplineSpawnStrategy SpawnStrategy;
    private float LastObserverPosition;

    private void Start()
    {
        var mesh2D = Mesh2D.BuildMesh2D();

        var map = new WiggleMap(3, Mathf.PI / 5, 4);

        SpawnStrategy = new SplineSpawnStrategy(mesh2D, Prefab, map, Spline, FragmentsLength, mesh2D.CalculateWidth(), true);

        for (int i = 0; i < MaxCount; i++)
        {
            SpawnStrategy.Spawn();
        }
    }

    private void Update()
    {
        var position = Observer.Position.z;
        var minPosition = Spline.LengthOffset;

        if (position - minPosition >= OffetToSpawn)
        {
            SpawnStrategy.Despawn();
            SpawnStrategy.Spawn();
        }
    }
}
