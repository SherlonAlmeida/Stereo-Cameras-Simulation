using System.IO;
using UnityEngine;

public class FiguresExport : MonoBehaviour
{
    public RenderTexture leftTexture;
    public RenderTexture rightTexture;

    public DepthRaycaster depthRaycaster; // Referência direta
    private Texture2D depthTexture;

    public string outputFolder = "Assets/Output";
    public string baseFilename = "image";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ExportAll();
        }
    }

    void ExportAll()
    {
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        SaveRenderTextureAsPNG(leftTexture, Path.Combine(outputFolder, baseFilename + "_left.png"));
        SaveRenderTextureAsPNG(rightTexture, Path.Combine(outputFolder, baseFilename + "_right.png"));

        // Exporta profundidade
        depthTexture = depthRaycaster.GetDepthTextureReal(); // ← exporta a real (em metros)
        string depthPathBase = Path.Combine(outputFolder, baseFilename + "_depth");

        SaveTexture2DAsEXR(depthTexture, depthPathBase + ".exr");        // valores reais em metros
        SaveDepthAsPNG(depthTexture, depthPathBase + ".png");            // visualização (tons de cinza)

        Debug.Log("✅ Exportação concluída!");
    }

    void SaveRenderTextureAsPNG(RenderTexture rt, string filePath)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        byte[] pngData = tex.EncodeToPNG();
        File.WriteAllBytes(filePath, pngData);

        RenderTexture.active = currentRT;
        Destroy(tex);
    }

    void SaveTexture2DAsEXR(Texture2D tex, string filePath)
    {
        byte[] exrData = tex.EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat);
        File.WriteAllBytes(filePath, exrData);
    }

    void SaveDepthAsPNG(Texture2D depthTex, string filePath)
    {
        int width = depthTex.width;
        int height = depthTex.height;

        Texture2D pngTex = new Texture2D(width, height, TextureFormat.RGB24, false);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float depth = depthTex.GetPixel(x, y).r;
                float normalized = Mathf.Clamp01(depth / 20f); // normaliza para tons de cinza (0m = preto, 20m = branco)
                pngTex.SetPixel(x, y, new Color(normalized, normalized, normalized));
            }
        }

        pngTex.Apply();
        byte[] pngBytes = pngTex.EncodeToPNG();
        File.WriteAllBytes(filePath, pngBytes);
        Destroy(pngTex);
    }
}
