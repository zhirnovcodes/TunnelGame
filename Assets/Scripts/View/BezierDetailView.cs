using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierDetailView : MonoBehaviour
{
    // todo vc
    public BezierDetailModel Model;

    public MeshRenderer Renderer;

    private int? _propertyId;
    private MaterialPropertyBlock _block;

    private void Awake()
    {
        Model.DrawInvoked += OnDrawInvoked;
    }

    private void OnDrawInvoked()
    {
        BezierExtentions.SendBezierToShader(Model.Data.Bezier, Renderer, ref _block, ref _propertyId);
    }
}
