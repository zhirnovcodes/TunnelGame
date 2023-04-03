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

    private TunnelSpawnStrategy SpawnStrategy;
    private float LastT;

    private void Awake()
    {
        Mesh2D mesh2D = new Mesh2D();
        Mesh2DFactory.CreateLine(mesh2D, Slices);//.CreateStrictEdge(mesh2D, Slices, true);//
        mesh2D.RotateClockWise(SpinDegrees * Mathf.Deg2Rad);
        mesh2D.Scale(Width);

        var map = new RoadMap(1000, Height, 1);

        SpawnStrategy = new TunnelSpawnStrategy(mesh2D, Prefab, map, Spline, FragmentsLength);

        for (int i = 0; i < MaxCount - 4; i++)
        {
            SpawnStrategy.Spawn();
        }
    }

    private void Update()
    {
        var t = Observer.Data.Position.z % 1;
        var shouldSpawn = t > LastT && LastT < 0.5f && t >= 0.5f;
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
