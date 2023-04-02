using UnityEngine;

public class BezierMapModel : MonoBehaviour
{
	public BezierData[] BezierArray;
	public byte[][] Options;

	public int Length = 1000;
	public int IndexFirst = 0;

	private BezierMapData Data;

	public void Initialize()
	{
		Data = new BezierMapData();
		Data.BezierArray = BezierArray;
		Data.PossibilityArray = Options;

		FillIndicies(Length, IndexFirst);
	}

	public int GetBezierIndex(int index)
	{
		return Data.IndiciesArray[index];
	}

	public BezierData GetBezier(int bezierIndex)
	{
		return Data.BezierArray[bezierIndex];
	}

	private void FillIndicies(int count, int indexFirst)
	{
		Data.IndiciesArray = new byte[count];

		var indexBefore = (byte)indexFirst;
		Data.IndiciesArray[0] = indexBefore;

		for (int i = 1; i < count; i++)
		{
			var options = Data.PossibilityArray[Data.IndiciesArray[i - 1]];
			var optionsCount = options.Length;

			Data.IndiciesArray[i] = options[Random.Range(0, optionsCount)];
		}
	}
}
