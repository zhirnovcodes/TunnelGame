using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllSidesTunnelMap : ITunnelMap, IScaleable
{
    private int[] _indicies;
    private BezierData[] _bezierArray;
    private List<int[]> _rules;
    private int _capacity;

    public float Scale { set; get; } = 1;

    public AllSidesTunnelMap(float scale = 1, int capacity = 100)
    {
        _capacity = capacity;

        _bezierArray = new BezierData[]
        {
            BezierFactory.BuildStraight(),
            BezierFactory.BuildCurved90(Vector3.up),
            BezierFactory.BuildCurved90(Vector3.down),
            BezierFactory.BuildCurved90(Vector3.left),
            BezierFactory.BuildCurved90(Vector3.right),
            //BezierFactory.BuildCurved(Vector3.up, angRadians),
            //BezierFactory.BuildCurved(Vector3.down, angRadians),
            //BezierFactory.BuildCurved(Vector3.left, angRadians),
            //BezierFactory.BuildCurved(Vector3.right, angRadians),
        };

        for (int i = 0; i < _bezierArray.Length; i++)
        {
            _bezierArray[i].Scale(scale);
        }

        /*
        _rules = new List<int[]>
        {
            new int[] { 0, 1, 2, 3, 4 },
            new int[] { 3, 4, 0 },
            new int[] { 3, 4, 0 },
            new int[] { 1, 2, 0 },
            new int[] { 1, 2, 0 },
        };
        */
        _rules = new List<int[]>
        {
            new int[] { 0, 1, 2, 3, 4 },
            new int[] { 0 },
            new int[] { 0 },
            new int[] { 0 },
            new int[] { 0 },
        };

        _indicies = new int[capacity];

        for (int i = 0; i < _capacity; i++)
        {
            _indicies[i] = GenerateIndex(i == 0 ? -1 : _indicies[i - 1]);
        }
    }

    public BezierData GetBezier(int bezierIndex)
    {
        var bezier = _bezierArray[bezierIndex];
        return bezier;
    }

    private int GenerateIndex(int lastIndex)
    {
        if (lastIndex < 0)
        {
            return 0;
        }

        var index = _rules[lastIndex][Random.Range(0, _rules[lastIndex].Count())];
        return index;
    }

	public int GetBezierIndex(int index)
    {
        index = Mathf.Max(0, index);
        return _indicies[index % _capacity];
    }

	public int GetBezierCount()
	{
        return _bezierArray.Length;

    }
}
