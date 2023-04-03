using UnityEngine;

public class RoadMap : ITunnelMap
{
    private BezierData[] Bezier;
    private int[][] Rules;
    private int[] Indicies;

    public RoadMap(int capacity, float height, float scale)
    {
        var up = BezierFactory.BuildHillUp(height);
        var down = BezierFactory.BuildHillDown(height);
        var right = BezierFactory.BuildCurved90(Vector3.right);
        var left = BezierFactory.BuildCurved90(Vector3.left);

        Bezier = new BezierData[]
        {
            BezierFactory.BuildStraight(), // 0
            up, // 1
            down,  // 2
            up, // 3 other part
            down, // 4
            right, // 5
            left  // 6
        };

        for (int i = 0; i < Bezier.Length; i++)
        {
            Bezier[i].Scale(scale);
        }

        Rules = new int[][]
        {
            new int[]{0, 0, 0, 0, 1, 2, 5, 6 }, // 0
            new int[]{0, 4 }, // 1
            new int[]{0, 3 }, // 2
            new int[]{0 }, // 3
            new int[]{0 }, // 4
            new int[]{0, 2, 3 }, // 5
            new int[]{0, 2, 3 }, // 6
        };

        Indicies = new int[capacity];

        for (int i = 0; i < capacity; i++)
        {
            Indicies[i] = GenerateIndex(i == 0 ? -1 : Indicies[i - 1]);
        }
    }

    public BezierData GetBezier(int bezierIndex)
    {
        return Bezier[bezierIndex];
    }

    public int GetBezierCount()
    {
        return Bezier.Length;
    }

    public int GetBezierIndex(int index)
    {
        return Indicies[index];
    }

    private int GenerateIndex(int lastIndex)
    {
        if (lastIndex < 0)
        {
            return 0;
        }

        var index = Rules[lastIndex][Random.Range(0, Rules[lastIndex].Length)];
        return index;
    }
}
