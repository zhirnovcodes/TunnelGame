using System.Collections.Generic;
using UnityEngine;

public class SplineSpawnStrategy
{
	public TunnelSplineModel Spline;
	public ISplineMap Map;
	public TunnelDetailObjectBuilder Builder;

	private readonly Spawner<BezierDetailModel> Spawner;
	private readonly Dictionary<int, Mesh> MeshLibrary = new Dictionary<int, Mesh>();

	public SplineSpawnStrategy(Mesh2D mesh, BezierDetailModel prefab, ISplineMap map, TunnelSplineModel spline, float fragmentLength)
	{
		Map = map;
		Spline = spline;

		Spawner = new Spawner<BezierDetailModel>(prefab);
		Builder = new TunnelDetailObjectBuilder();

		BuildMeshes(mesh, fragmentLength, map);
	}

	private void BuildMeshes(Mesh2D mesh, float fragmentLength, ISplineMap map)
	{
		const float piecesCalcLength = 0.01f;

		var piecesCount = Mathf.FloorToInt(Mathf.Max(1, fragmentLength / piecesCalcLength));

		var parametrizationMap = new SplineParametrizationMap(fragmentLength);

		var mesh3D = new Mesh3D();

		for (int i = 0; i < map.GetBezierCount(); i++)
		{
			var bezier = map.GetBezier(i);
			var fragments = Mathf.Max(1, Mathf.CeilToInt(bezier.Length / fragmentLength));

			parametrizationMap.Clear();

			var tStart = 0f;
			parametrizationMap.Append(tStart);
			for (int f = 0; f <= fragments; f++)
			{
				tStart = Mathf.Clamp01( bezier.GetTFromLength(fragmentLength, tStart, piecesCount));
				parametrizationMap.Append(tStart);
			}

			Mesh3DFactory.GenetareMesh3D(mesh3D, mesh, fragments);
			BezierExtentions.BendMeshWithBezier(mesh3D, bezier, parametrizationMap);

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
