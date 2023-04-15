using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchObjectState : MonoBehaviour
{
    public Transform Parent;
    public Transform Target;

    void Start()
    {
        
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(Target.position - transform.position, (Parent?.rotation ?? Quaternion.identity) * Vector3.up);
    }
}
