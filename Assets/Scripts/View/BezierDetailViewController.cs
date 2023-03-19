using UnityEngine;

public class BezierDetailViewController : MonoBehaviour
{
    public BezierDetailModel Model;
    public BezierDetailView View;

    private void Awake()
    {
        Model.DrawInvoked += OnDrawInvoked;
    }

    private void OnDrawInvoked()
    {
        var tilingX = 1;
        var tilingY = 1;
        var offsetX = 0;
        var offsetY = 0;
        var textureOffset = new Vector4(tilingX, tilingY, offsetX, offsetY);

        View.SetTextureOffset(textureOffset);
    }

    [System.Serializable]
    public class BezierDetailView
    {
        public MeshRenderer Renderer;

        private int? _mainTexPropertyId;
        private float? _textureProportions;
        private MaterialPropertyBlock _block;

        public float GetTextureProportions
        {
            get
            {
                if (_textureProportions == null)
                {
                    var property = Shader.PropertyToID("_MainTex");
                    var texture = Renderer.sharedMaterial.GetTexture(property);
                    _textureProportions = (float)texture.width / texture.height;
                }
                return _textureProportions.Value;
            }
        }

        public void SetTextureOffset(Vector4 offset)
        {
            if (_mainTexPropertyId == null)
            {
                _mainTexPropertyId = Shader.PropertyToID("_MainTex_ST");
            }
            if (_block == null)
            {
                _block = new MaterialPropertyBlock();
            }

            _block.SetVector(_mainTexPropertyId.Value, offset);
            Renderer.SetPropertyBlock(_block);
        }
    }

}