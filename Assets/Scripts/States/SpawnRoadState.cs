using UnityEngine;

public class SpawnRoadState : MonoBehaviour
{
    public TunnelSplineModel Spline;
    public MapBase Map;
    public BezierDetailModel Prefab;
    public int MaxCount = 5;

    public int Slices = 3;
    public float FragmentsLength = 0.2f;
    public float Width = 5;
    public float SpinDegrees = 0;

    private TunnelSpawnStrategy SpawnStrategy;

    private void Awake()
    {
        Mesh2D mesh2D = new Mesh2D();
        Mesh2DFactory.CreateStrictEdge(mesh2D, Slices, true);//.CreateLine(mesh2D, Slices);//
        mesh2D.RotateClockWise(SpinDegrees * Mathf.Deg2Rad);
        mesh2D.Scale(Width);

        SpawnStrategy = new TunnelSpawnStrategy(mesh2D, Prefab, Map.Map, Spline, FragmentsLength);

        for (int i = 0; i < MaxCount - 2; i++)
        {
            SpawnStrategy.Spawn();
        }
    }

    private void Update()
    {
        var shouldSpawn = Input.GetKeyDown(KeyCode.Return);
        var shouldDespawn = Input.GetKeyDown(KeyCode.Space);

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
