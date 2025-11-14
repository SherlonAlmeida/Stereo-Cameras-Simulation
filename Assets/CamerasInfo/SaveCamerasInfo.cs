using UnityEngine;
using System.IO;

public class SaveCamerasInfo : MonoBehaviour
{
    [System.Serializable]
    public class IntrinsicData
    {
        public int width;
        public int height;
        public float fx;
        public float fy;
        public float cx;
        public float cy;
    }

    public Camera cam;
    public string outputFolder = "Assets/Output";
    public string extFilename = "camera_extrinsics.log";
    public string intFilename = "camera_intrinsics.json";
    public int frameIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            ExportPose(cam, frameIndex, Path.Combine(outputFolder, extFilename));
            ExportIntrinsics(cam, Path.Combine(outputFolder, intFilename));
            frameIndex++;
        }
    }

    void ExportPose(Camera cam, int index, string path)
    {
        // camera -> world (em Unity)
        Matrix4x4 T_wc = Matrix4x4.TRS(cam.transform.position, cam.transform.rotation, Vector3.one);

        // Formatar saída
        string header = $"{index}\t{index}\t{index + 1}\n";
        string mat =
            $"   {T_wc.m00,10:F6} {T_wc.m01,10:F6} {T_wc.m02,10:F6} {T_wc.m03,10:F6}\n" +
            $"   {T_wc.m10,10:F6} {T_wc.m11,10:F6} {T_wc.m12,10:F6} {T_wc.m13,10:F6}\n" +
            $"   {T_wc.m20,10:F6} {T_wc.m21,10:F6} {T_wc.m22,10:F6} {T_wc.m23,10:F6}\n" +
            $"   {0,10} {0,10} {0,10} {1,10}\n";

        File.AppendAllText(path, header + mat);
        Debug.Log($"[SaveCamerasInfo] Extrinsics {index} saved to {path}");
        Debug.Log($"[SaveCamerasInfo] Extrinsics {index} saved to {path}");
    }

    void ExportIntrinsics(Camera cam, string path)
    {
        int width = cam.pixelWidth;
        int height = cam.pixelHeight;

        // Cálculo de intrínsecos baseado no FOV e aspect ratio da câmera
        float fy = (height / 2f) / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float fx = fy * cam.aspect;
        float cx = width / 2f;
        float cy = height / 2f;

        // JSON compatível com o formato Open3D
        string json =
$@"{{
    ""width"": {width},
    ""height"": {height},
    ""intrinsic_matrix"": [
        [{fx:F6}, 0.0, {cx:F6}],
        [0.0, {fy:F6}, {cy:F6}],
        [0.0, 0.0, 1.0]
    ]
}}";

        File.WriteAllText(path, json);
        Debug.Log($"[SaveCamerasInfo] Intrinsics saved to {path}");
    }
}
