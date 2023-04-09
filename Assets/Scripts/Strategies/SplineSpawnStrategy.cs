using System.Collections.Generic;
using UnityEngine;

public class SplineSpawnStrategy
{
	public TunnelSplineModel Spline;
	public ISplineMap Map;

	private readonly Spawner<BezierDetailModel> Spawner;
	private readonly Dictionary<int, TunnelDetailObjectBuilder> BuilderLibrary = new Dictionary<int, TunnelDetailObjectBuilder>();

	public SplineSpawnStrategy(Mesh2D mesh, BezierDetailModel prefab, ISplineMap map, TunnelSplineModel spline, float fragmentLength, float width)
	{
		Map = map;
		Spline = spline;

		Spawner = new Spawner<BezierDetailModel>(prefab);

		BuildMeshes(mesh, fragmentLength, map, width);
	}

	private void BuildMeshes(Mesh2D mesh, float fragmentLength, ISplineMap map, float width)
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

			while (tStart <= 1)
			{
				tStart = bezier.GetTFromLength(fragmentLength, tStart, piecesCount);
				parametrizationMap.Append(tStart);
			}

			Mesh3DFactory.GenetareMesh3D(mesh3D, mesh, fragments + 1);
			BezierExtentions.BendMeshWithBezier(mesh3D, bezier, parametrizationMap); // TODO parametrizationMap

			var builder = new TunnelDetailObjectBuilder().
				WithMesh(mesh3D.ToMesh()).
				WithBezierData(bezier).
				WithWidth(width);

			BuilderLibrary.Add(i, builder);
		}
	}

	public void Spawn()
	{
		var nextBezierIndex = Map.GetBezierIndex(Spline.MaxIndex + 1);
		var nextBezier = Map.GetBezier(nextBezierIndex);
		var oldLengthOffset = Spline.LengthOffset + Spline.SplineLength;

		Spline.Append(nextBezier);

		var newSplinePosition = new SplinePositionData { Position = new Vector3(0, 0, Spline.MaxIndex) };
		var newWorldPosition = Spline.GetWorldPositionRotation(newSplinePosition);
		var model = Spawner.Spawn();
		var builder = BuilderLibrary[nextBezierIndex];

		builder.
			WithWorldPositionRotation(newWorldPosition).
			WithBezierDetailModel(model).
			WithLengthOffset(oldLengthOffset).
			Build();
	}

	public void Despawn()
	{
		Spline.Pop();
		Spawner.Despawn();
	}
}
