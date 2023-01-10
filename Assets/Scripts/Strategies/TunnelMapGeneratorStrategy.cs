using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            directionTwo = Vector3.right;
        }
        else if (directionTwo.x >= 1)
        {
            directionTwo = Vector3.forward;
        }
    }
}
