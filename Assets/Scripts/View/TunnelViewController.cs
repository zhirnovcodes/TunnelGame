using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelViewController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Color _color = Color.white;

    private int _bezierPropertyId = -1;
    private int _textureScalePropertyId = -1;

    private Material _material = null;

    private int TextureScalePropertyId
    {
        get
        {
            _textureScalePropertyId = _textureScalePropertyId == -1 ? Shader.PropertyToID("_MainTex_ST") : _textureScalePropertyId;
            return _textureScalePropertyId;
        }
    }

    private int BezierPropertyId
    {
        get
        {
            _bezierPropertyId = _bezierPropertyId == -1 ? Shader.PropertyToID("_BezierNodes") : _bezierPropertyId;
            return _bezierPropertyId;
        }
    }

    private Material Material
    {
        get
        {
            if (_material == null)
            {
                _material = Application.isPlaying ? _renderer.material : _renderer.sharedMaterial;
            }
            return _material;
        }
    }


    public Vector4 TextureOffset
    {
        get
        {
            return Material.GetVector(TextureScalePropertyId);
        }
        set
        {
            Material.SetVector(TextureScalePropertyId, value);
        }
    }

    public BezierData Bezier
    {
        set
        {
            var bezier = value;
            var matrix = new Matrix4x4();
            matrix.SetRow(0, bezier.P0);
            matrix.SetRow(1, bezier.P1);
            matrix.SetRow(2, bezier.P2);
            var lastRow = new Vector4(bezier.P3.x, bezier.P3.y, bezier.P3.z, bezier.PointsCount);
            matrix.SetRow(3, lastRow);

            Material.SetMatrix(BezierPropertyId, matrix);
        }
    }

    private void Awake()
    {
        Material.SetColor("_Color", _color);
    }

    /*
    private MaterialPropertyBlock _block;
    private int _bezierPropertyId = -1;
    private int _textureScalePropertyId = -1;

    private MaterialPropertyBlock Block
    {
        get
        {
            _block = _block ?? new MaterialPropertyBlock();
            return _block;
        }
    }

    public Vector4 TextureOffset 
    { 
        get
        {
            _renderer.GetPropertyBlock(Block);
            
            return Block.GetVector(TextureScalePropertyId);
        } 
        set 
        {
            Block.SetVector(TextureScalePropertyId, value);
        } 
    }

    public BezierData Bezier 
    { 
        set
        {
            var bezier = value;
            var matrix = new Matrix4x4();
            matrix.SetRow(0, bezier.P0);
            matrix.SetRow(1, bezier.P1);
            matrix.SetRow(2, bezier.P2);
            var lastRow = new Vector4(bezier.P3.x, bezier.P3.y, bezier.P3.z, bezier.PointsCount);
            matrix.SetRow(3, lastRow);

            Block.SetMatrix(BezierPropertyId, matrix);
        }
    }

    public void SendToShader()
    {
        _renderer.SetPropertyBlock(Block);
    }
    */
}
