using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllSidesMapStrategy : IMapStrategy, IScaleable
{
    private int[] _indicies;
    private TunnelDetailData[] _dictionary;
    private List<int[]> _rules;
    private int _capacity;

    public float Scale { set; get; } = 1;

    public AllSidesMapStrategy(int capacity = 100)
    {
        _capacity = capacity;

        _dictionary = new TunnelDetailData[]
        {
            TunnelDetailFactory.BuildStraight(),
            TunnelDetailFactory.BuildCurved(Vector3.up),
            TunnelDetailFactory.BuildCurved(Vector3.down),
            TunnelDetailFactory.BuildCurved(Vector3.left),
            TunnelDetailFactory.BuildCurved(Vector3.right),
        };

        _rules = new List<int[]>
        {
            new int[] { 0, 1, 2, 3, 4 },
            new int[] { 3, 4, 0 },
            new int[] { 3, 4, 0 },
            new int[] { 1, 2, 0 },
            new int[] { 1, 2, 0 },
        };

        _indicies = new int[capacity];

        for (int i = 0; i < _capacity; i++)
        {
            _indicies[i] = GenerateIndex(i == 0 ? -1 : _indicies[i - 1]);
        }
    }

    public TunnelDetailData GetDetail(int index)
    {
        index = Mathf.Max(0, index);
        return _dictionary[_indicies[index % _capacity]] * Scale;
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
