using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Webcam : MonoBehaviour
{
    //[ReadOnly] public List<string> webcams = new List<string>();
    [SerializeField] int webcamId;
    private WebCamTexture texture = null;
    public int width;
    public int height;
    private bool textureCached;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public WebCamDevice GetDeviceInfo()
    {
        CheckWebcamId();
        return WebCamTexture.devices[webcamId];
    }

    public WebCamTexture GetTexture()
    {
        if (texture == null)
        {
            WebCamDevice device = GetDeviceInfo();
            texture = new WebCamTexture(device.name, width, height);
        }

        return texture;
    }

    private void CheckWebcamId()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length < 1)
        {
            return;
        }

        if (webcamId < 0 || webcamId >= devices.Length)
        {
            Debug.LogWarning("Webcam id of " + webcamId + " is not valid. Defaulting to webcam 0");
            webcamId = 0;
        }
    }

    private void UpdateWebcamList()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
       
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateWebcamList();
        //CheckWebcamId();
    }
}
