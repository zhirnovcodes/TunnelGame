using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView, RequireComponent(typeof(Camera))]
public class CameraPostProcessing : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private bool _shouldUseDepth = true;

    private void Start()
    {
        if (_shouldUseDepth)
        {
            GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material == null)
        {
            return;
        }
        if (_material == null)
        {
            return;
        }
        Graphics.Blit(source, destination, _material);
    }
}
