using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnTunnelState : MonoBehaviour
{
    public SplinePositionModel Observer;
    public TunnelSplineModel Spline;
    public MapBase Map;
    public Spawner<BezierDetailModel> Spawner;
    public BezierDetailModel Prefab;
    public TunnelDetailObjectBuilder Builder;
    public int MaxCount = 5;
    [Range(0, 1)] public float SpawnPosition = 0.2f;

    public int Slices = 3;
    public int Fragments = 8;
    public float Radius = 5;
    public bool ShadedInside = true;
    public float SpinDegrees = 0;

    private float LastT;
    private Mesh2D Mesh2D = new Mesh2D();
    private Mesh3D Mesh3D = new Mesh3D();
    private Dictionary<int, Mesh> MeshLibrary = new Dictionary<int, Mesh>();

    private void Awake()
    {
        Spawner = new Spawner<BezierDetailModel>(Prefab);
        Builder = new TunnelDetailObjectBuilder();

        Mesh2DFactory.FillCircleMesh2D(Mesh2D, Slices, ShadedInside, Radius, SpinDegrees);

        for (int i = 0; i < MaxCount - 1; i++)
        {
            SpawnDetail();
        }
    }

    private void OnEnable()
    {
        
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
            Spline.Pop();
            Spawner.Despawn();
        }

        if (shouldSpawn)
        {
            SpawnDetail();
        }
    }

    private void SpawnDetail()
    {
        var nextBezier = Map.Strategy.GetBezier(Spline.MaxIndex + 1);

        Spline.Append(nextBezier);

        var newSplinePosition = new SplinePositionData { Position = new Vector3(0, 0, Spline.MaxIndex) };
        var newWorldPosition = Spline.GetWorldPositionRotation(newSplinePosition);
        var model = Spawner.Spawn();
        var mesh = GetMesh(nextBezier);

        Builder.
            WithWorldPositionRotation(newWorldPosition).
            WithBezierDetailModel(model).
            WithMesh(mesh).
            Build();
    }

    private Mesh GetMesh(BezierData bezier)
    {
        //bezier.ResetPosition();

        var bezierKey = GetBezierHashCode(bezier);

        if (!MeshLibrary.TryGetValue(bezierKey, out var mesh))
        {
            mesh = new Mesh();
            Mesh3DFactory.FillTunnel3D(Mesh3D, Mesh2D, bezier, Fragments, Radius);
            Mesh3D.ToMesh(ref mesh);
            MeshLibrary.Add(bezierKey, mesh);
        }

        return mesh;
    }

    private int GetBezierHashCode(BezierData bezier)
    {
        var v = bezier.P2 - bezier.P0;
        var vInt = new Vector3Int((int)v.x, (int)v.y, (int)v.z);

        return vInt.GetHashCode();
    }
}
