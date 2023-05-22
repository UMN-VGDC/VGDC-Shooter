using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamViewer : MonoBehaviour
{
    [SerializeField] Webcam webcam;

    RawImage webcamPreview;
    private WebCamTexture texture;
    // Start is called before the first frame update
    void Start()
    {
        texture = webcam.GetTexture();
        webcamPreview = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        webcamPreview.texture = texture;
    }
}
