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

    private float LastObserverPosition;

    private SplineSpawnStrategy SpawnStrategy;

    private void Awake()
    {
        Mesh2D mesh2D = null;
        Mesh2DFactory.CreateCircleMesh2D(mesh2D, Slices, ShadedInside, Radius);
        mesh2D.RotateClockWise(SpinDegrees * Mathf.Deg2Rad);

        SpawnStrategy = new SplineSpawnStrategy(mesh2D, Prefab, Map.Map, Spline, FragmentsLength, MathP.TAU * Radius);

        for (int i = 0; i < MaxCount - 2; i++)
        {
            SpawnStrategy.Spawn();
        }
    }

    private void Update()
    {
        var position = Observer.Data.Position.z;

        //var shouldSpawn = Input.GetKeyDown(KeyCode.Return);
        //var shouldDespawn = Input.GetKeyDown(KeyCode.Space);
        //var shouldSpawn = position > LastObserverPosition && LastObserverPosition < SpawnPosition && t >= SpawnPosition;
        //var shouldDespawn = position > LastObserverPosition && (position > Spline.LengthOffset + );


        if (position >= Spline.LengthOffset + 4)
        {
            LastObserverPosition = position;

            SpawnStrategy.Despawn();
            SpawnStrategy.Spawn();
        }
        /*
        if (shouldDespawn)
        {
            SpawnStrategy.Despawn();
        }

        if (shouldSpawn)
        {
            SpawnStrategy.Spawn();
        }*/
    }
}
