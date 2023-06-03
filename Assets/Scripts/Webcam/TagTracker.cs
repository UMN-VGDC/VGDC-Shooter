using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TagTracker : MonoBehaviour
{
    public delegate void OnTracked(Vector2 screenPos);

    public OnTracked OnTrack;
    AprilTag.TagDetector detector;
    [SerializeField] Webcam webcam;
    [SerializeField] int decimation;
    [SerializeField] float tagSize;
    [SerializeField] float fov;
    [SerializeField] Vector2 screenSize;
    [SerializeField] float planeDistance;

    [SerializeField] [Range(0, 1)] float lerpSpeed;


    private Vector2 rawScreenPos;

    private Vector2 dampedScreenPos;

    [SerializeField] private bool usingWebCam;


    private Plane plane;
    private WebCamTexture texture;

    // Start is called before the first frame update
    void Start()
    {

        rawScreenPos = Vector2.one * 0.5f;
        dampedScreenPos = rawScreenPos;
        if(usingWebCam)
        {
            texture = webcam.GetTexture();

            texture.Play();
            detector = new AprilTag.TagDetector(texture.width, texture.height, decimation);
            plane = new Plane(Vector3.forward, planeDistance);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {


        // AprilTag detection


        //Debug.Log(viewPortPoint);
        if (usingWebCam)
        {
            UpdateRawScreenPos();
            
        } else
        {
            rawScreenPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        DampScreenPos();



    }

    private void UpdateRawScreenPos()
    {
        var fovRad = Camera.main.fieldOfView * Mathf.Deg2Rad;
        detector.ProcessImage(texture.AsSpan(), fovRad, tagSize);


        if (detector.DetectedTags.Count() < 1)
        {
            return;
        }
        AprilTag.TagPose pose = detector.DetectedTags.First();

        Vector3 direction = pose.Rotation * Vector3.back;
        Vector3 intersection;

        bool isValid;
        (intersection, isValid) = GetIntersectionPoint(pose.Position, direction);
        if (!isValid)
        {
            return;
        }
        Vector2 screenPos = intersection;
        screenPos.x /= screenSize.x;
        screenPos.y /= screenSize.y;
        screenPos += 0.5f * Vector2.one;
        screenPos.x = 1 - screenPos.x;
        screenPos.x = Mathf.Clamp(screenPos.x, 0, 1);
        screenPos.y = Mathf.Clamp(screenPos.y, 0, 1);

        rawScreenPos = screenPos;

    }

    private void DampScreenPos()
    {
        dampedScreenPos = Vector2.Lerp(dampedScreenPos, rawScreenPos, lerpSpeed);
    
    }

    public Vector2 GetDampedScreenPos()
    {
        return dampedScreenPos;
    }

    public (Vector3, bool) GetIntersectionPoint(Vector3 position, Vector3 direction)
    {
        float enter;
        Ray ray = new Ray(position, direction);
        if (plane.Raycast(ray, out enter))
        {
            
            return (ray.GetPoint(enter), true);
        }
        return (Vector3.zero, false);

    }
    public void OnDestroy()
    {
        if(usingWebCam)
        {
            detector.Dispose();
        }
    }
}
