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
        // Matriz mundo -> câmera
        Matrix4x4 T_wc = Matrix4x4.TRS(cam.transform.position, cam.transform.rotation, Vector3.one);
        Matrix4x4 T_cw = T_wc.inverse;

        // Corrigir sistema de coordenadas (Unity -> Open3D)
        Matrix4x4 flipZ = Matrix4x4.Scale(new Vector3(1, 1, -1));
        T_cw = flipZ * T_cw * flipZ;

        // Formatar como no dataset Fountain
        string header = $"{index}\t0\t1\n";
        string mat =
            $"   {T_cw.m00,10:F6} {T_cw.m01,10:F6} {T_cw.m02,10:F6} {T_cw.m03,10:F6}\n" +
            $"   {T_cw.m10,10:F6} {T_cw.m11,10:F6} {T_cw.m12,10:F6} {T_cw.m13,10:F6}\n" +
            $"   {T_cw.m20,10:F6} {T_cw.m21,10:F6} {T_cw.m22,10:F6} {T_cw.m23,10:F6}\n" +
            $"   {0,10} {0,10} {0,10} {1,10}\n";

        File.AppendAllText(path, header + mat);
        Debug.Log($"Extrinsics {index} saved at {path}.");
    }

    void ExportIntrinsics(Camera cam, string path)
    {
        int width = cam.pixelWidth;
        int height = cam.pixelHeight;

        float fy = (height / 2f) / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float fx = fy * cam.aspect;
        float cx = width / 2f;
        float cy = height / 2f;

        // JSON manual para manter formato compatível com Open3D
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
        Debug.Log($"Intrinsics saved at {path}.");
    }
}
