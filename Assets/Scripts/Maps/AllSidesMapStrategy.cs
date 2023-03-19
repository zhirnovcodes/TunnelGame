using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllSidesMapStrategy : IMapStrategy, IScaleable
{
    private int[] _indicies;
    private BezierData[] _dictionary;
    private List<int[]> _rules;
    private int _capacity;

    public float Scale { set; get; } = 1;

    public AllSidesMapStrategy(int capacity = 100)
    {
        _capacity = capacity;

        var angRadians = Mathf.Deg2Rad * 45f;

        _dictionary = new BezierData[]
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

    public BezierData GetBezier(int index)
    {
        index = Mathf.Max(0, index);
        var bezier = _dictionary[_indicies[index % _capacity]];
        bezier.Scale(Scale);
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
}
