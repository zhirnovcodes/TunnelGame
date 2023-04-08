using UnityEngine;

public class BezierDetailViewController : MonoBehaviour
{
    public BezierDetailModel Model;
    public BezierDetailView View;

    private float? TextureProportions;

    private void Awake()
    {
        Model.DrawInvoked += OnDrawInvoked;

        TextureProportions = View.GetTextureProportions();
    }

    private void OnDrawInvoked(BezierDelailData data)
    {
        var proportions = TextureProportions ?? View.GetTextureProportions();

        var width = data.Width;
        var height = width / proportions;

        var tilingX = 1f;
        var tilingY = data.Length / height;

        var offsetY = data.LengthOffset / height;

        var tiling = new Vector2(tilingX, tilingY);
        var offset = new Vector2(0, offsetY);
        var textureOffset = new Vector4(tiling.x, tiling.y, offset.x, offset.y);

        View.SetTextureOffset(textureOffset);
    }

    [System.Serializable]
    public class BezierDetailView
    {
        public MeshRenderer Renderer;

        private int? _mainTexPropertyId;
        private MaterialPropertyBlock _block;

        private int? _albedoTexPropertyId;

        public float GetTextureProportions()
        {
            var property = Shader.PropertyToID("_MainTex");
            var texture = Renderer.sharedMaterial.GetTexture(property);
            return (float)texture.width / texture.height;
        }

        public void SetTextureOffset(Vector4 offset)
        {
            if (_mainTexPropertyId == null)
            {
                _mainTexPropertyId = Shader.PropertyToID("_MainTex_ST");
            }
            if (_albedoTexPropertyId == null)
            {
                _albedoTexPropertyId = Shader.PropertyToID("_DetailAlbedoMap_ST");
            }
            if (_block == null)
            {
                _block = new MaterialPropertyBlock();
            }

            _block.SetVector(_mainTexPropertyId.Value, offset);
            _block.SetVector(_albedoTexPropertyId.Value, offset);
            Renderer.SetPropertyBlock(_block);
        }
    }

}