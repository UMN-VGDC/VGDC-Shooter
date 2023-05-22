using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ReadOnlyAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
[ExecuteInEditMode]
public class Webcam : MonoBehaviour
{
    [ReadOnly] public List<string> webcams = new List<string>();
    [SerializeField] int webcamId;
    private WebCamTexture texture = null;
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
            texture = new WebCamTexture(device.name);
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
        webcams = new List<string>();
        for (int i = 0; i < devices.Length; i++)
        {
            webcams.Add(i + ": " + devices[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWebcamList();
        CheckWebcamId();
    }
}
