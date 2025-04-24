using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DepthRaycaster : MonoBehaviour
{
    public Camera cam;
    public Material previewMaterial;

    public int resolutionWidth = 384;
    public int resolutionHeight = 256;
    public float maxDepthMeters = 20f; // usado para visualização

    private Texture2D depthMapReal;     // em metros, exportável
    private Texture2D depthMapPreview;  // normalizado [0-1], para o Quad

    void Start()
    {
        if (cam == null)
            cam = GetComponent<Camera>();

        depthMapReal = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RFloat, false);
        depthMapPreview = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);

        if (previewMaterial != null)
            previewMaterial.mainTexture = depthMapPreview;
    }

    void Update()
    {
        for (int y = 0; y < resolutionHeight; y++)
        {
            for (int x = 0; x < resolutionWidth; x++)
            {
                float u = (float)x / (resolutionWidth - 1);
                float v = (float)y / (resolutionHeight - 1);
                Ray ray = cam.ViewportPointToRay(new Vector3(u, v, 0));

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    float depth = hit.distance;

                    // depth real em metros
                    depthMapReal.SetPixel(x, y, new Color(depth, 0, 0));

                    // normalizado para visualização
                    float norm = Mathf.Clamp01(depth / maxDepthMeters);
                    depthMapPreview.SetPixel(x, y, new Color(norm, norm, norm));
                }
                else
                {
                    depthMapReal.SetPixel(x, y, new Color(0, 0, 0));
                    depthMapPreview.SetPixel(x, y, new Color(0, 0, 0));
                }
            }
        }

        depthMapReal.Apply();
        depthMapPreview.Apply();

        if (previewMaterial != null)
            previewMaterial.mainTexture = depthMapPreview;
    }

    public Texture2D GetDepthTextureReal()
    {
        return depthMapReal;
    }

    public Texture2D GetDepthTexturePreview()
    {
        return depthMapPreview;
    }
}
