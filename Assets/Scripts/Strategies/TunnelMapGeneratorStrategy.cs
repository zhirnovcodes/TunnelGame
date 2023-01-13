using UnityEngine;

public enum MapStrategyType
{
    Default,
    Curl
}

public interface ITunnelMapGeneratorStrategy
{
    void Generate(ref Vector3 directionOne, ref Vector3 directionTwo);
}

public class TunnelMapGeneratorStrategy : ITunnelMapGeneratorStrategy
{
    public void Generate(ref Vector3 directionOne, ref Vector3 directionTwo)
    {
        directionOne = directionTwo;

        if (directionTwo.z >= 1)
        {
            var r = Random.Range(0, 5);
            switch (r)
            {
                case 0:
                    directionTwo = Vector3.right; break;
                case 1:
                    directionTwo = Vector3.left; break;
                case 2:
                    directionTwo = Vector3.down; break;
                case 3:
                    directionTwo = Vector3.up; break;
                case 4:
                    directionTwo = Vector3.forward; break;
            }
        }
        else if (Mathf.Abs(directionTwo.y) >= 1)
        {
            directionTwo = Vector3.forward;
        }
        else if (Mathf.Abs(directionTwo.x) >= 1)
        {
            directionTwo = Vector3.forward;
        }
    }
}

public class TunnelCurlGeneratorStrategy : ITunnelMapGeneratorStrategy
{
    public void Generate(ref Vector3 directionOne, ref Vector3 directionTwo)
    {
        directionOne = directionTwo;
        if (directionTwo.y > 0)
        {
            directionTwo = Vector3.forward;
        }
        else
        {
            directionTwo = Vector3.up;
        }
    }
}