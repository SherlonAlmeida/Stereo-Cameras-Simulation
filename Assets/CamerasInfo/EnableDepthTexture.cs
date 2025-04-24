using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EnableDepthTexture : MonoBehaviour
{
    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }
}