using System.Collections.Generic;
using UnityEngine;

public class TunnelSpawnStrategy
{
	public TunnelSplineModel Spline;
	public ITunnelMap Map;
	public TunnelDetailObjectBuilder Builder;

	private readonly Spawner<BezierDetailModel> Spawner;
	private readonly Dictionary<int, Mesh> MeshLibrary = new Dictionary<int, Mesh>();

	public TunnelSpawnStrategy(Mesh2D mesh, BezierDetailModel prefab, ITunnelMap map, TunnelSplineModel spline, float fragmentLength)
	{
		Map = map;
		Spline = spline;

		Spawner = new Spawner<BezierDetailModel>(prefab);
		Builder = new TunnelDetailObjectBuilder();

		var mesh3D = new Mesh3D();

		for (int i = 0; i < map.GetBezierCount(); i++)
		{
			var bezier = map.GetBezier(i);
			var fragments = Mathf.Max(1, Mathf.CeilToInt(bezier.Length / fragmentLength));

			Mesh3DFactory.GenetareMesh3D(mesh3D, mesh, fragments);
			BezierExtentions.BendMeshWithBezier(mesh3D, bezier);

			MeshLibrary.Add(i, mesh3D.ToMesh());
		}
	}


	public void Spawn()
	{
		var nextBezierIndex = Map.GetBezierIndex(Spline.MaxIndex + 1);
		var nextBezier = Map.GetBezier(nextBezierIndex);

		Spline.Append(nextBezier);

		var newSplinePosition = new SplinePositionData { Position = new Vector3(0, 0, Spline.MaxIndex) };
		var newWorldPosition = Spline.GetWorldPositionRotation(newSplinePosition);
		var model = Spawner.Spawn();
		var mesh = MeshLibrary[nextBezierIndex];

		Builder.
			WithWorldPositionRotation(newWorldPosition).
			WithBezierDetailModel(model).
			WithMesh(mesh).
			Build();
	}

	public void Despawn()
	{
		Spline.Pop();
		Spawner.Despawn();
	}
}
