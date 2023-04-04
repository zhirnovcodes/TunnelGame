using UnityEngine;

public class SpawnTunnelState : MonoBehaviour
{
    public SplinePositionModel Observer;
    public TunnelSplineModel Spline;
    public MapBase Map;
    public BezierDetailModel Prefab;
    public int MaxCount = 5;
    [Range(0, 1)] public float SpawnPosition = 0.2f;

    public int Slices = 3;
    public float FragmentsLength = 0.5f;
    public float Radius = 5;
    public bool ShadedInside = true;
    public float SpinDegrees = 0;

    private float LastT;

    private SplineSpawnStrategy SpawnStrategy;

    private void Awake()
    {
        Mesh2D mesh2D = null;
        Mesh2DFactory.CreateCircleMesh2D(mesh2D, Slices, ShadedInside, Radius);
        mesh2D.RotateClockWise(SpinDegrees * Mathf.Deg2Rad);

        SpawnStrategy = new SplineSpawnStrategy(mesh2D, Prefab, Map.Map, Spline, FragmentsLength);

        for (int i = 0; i < MaxCount - 2; i++)
        {
            SpawnStrategy.Spawn();
        }
    }

    private void Update()
    {
        var t = Observer.Data.Position.z % 1;

        //var shouldSpawn = Input.GetKeyDown(KeyCode.Return);
        //var shouldDespawn = Input.GetKeyDown(KeyCode.Space);
        var shouldSpawn = t > LastT && LastT < SpawnPosition && t >= SpawnPosition;
        var shouldDespawn = (Spline.Count + (shouldSpawn ? 1 : 0)) > MaxCount;

        LastT = t;

        if (shouldDespawn)
        {
            SpawnStrategy.Despawn();
        }

        if (shouldSpawn)
        {
            SpawnStrategy.Spawn();
        }
    }
}
