using UnityEngine;

public class CameraRender : MonoBehaviour
{
    [SerializeField] public Light[] cameraLight;

    private void OnPreRender()
    {
        for (int i = 0; i < cameraLight.Length; i++)
        {
            cameraLight[i].enabled = false;
        }
    }

    private void OnPreCull()
    {
        for (int i = 0; i < cameraLight.Length; i++)
        {
            cameraLight[i].enabled = false;
        }
    }

    private void OnPostRender()
    {
        for (int i = 0; i < cameraLight.Length; i++)
        {
            cameraLight[i].enabled = true;
        }
    }
}
