using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;

using System.Threading;

public class CameraReciever : MonoBehaviour
{
    public static CameraReciever instance = null;
    Thread udpClientThread = null;
    UdpClient client = null;
    [SerializeField] private int port;
    [SerializeField] private bool usingCamera = false;
    [SerializeField] Vector2 screenSize;
    [SerializeField] float planeDistance;
    [SerializeField] [Range(0, 1)] float lerpSpeed;
    private Plane plane;

    private ConcurrentQueue<string> data_queue;

    private bool recievingData;
    private string data;
    private Quaternion rot;
    private Vector3 translation;

    private Vector3 rawScreenPos;
    private Vector3 dampedScreenPos;
    // Start is called before the first frame update
    private void Awake()
    { 
        if (instance == null)
        {
            data_queue = new ConcurrentQueue<string>();
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (usingCamera)
            {
                udpClientThread = new Thread(new ThreadStart(RecieveData));
                udpClientThread.IsBackground = true;
                udpClientThread.Start();
            }
            data = "";
            plane = new Plane(Vector3.forward, planeDistance);
            rot = Quaternion.identity;
            translation = Vector3.zero;

        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
        
        
        
    }
    void Start()
    {
        
    }

    private void RecieveData()
    {
        client = new UdpClient(port);
        while(usingCamera)
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

                string new_data = Encoding.UTF8.GetString(client.Receive(ref endPoint));
                if(new_data != data)
                {
                    data_queue.Enqueue(new_data);
                    data = new_data;
                }
            }catch(Exception err)
            {
                Debug.LogError(err.ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (usingCamera)
        {
            ParseData();
            UpdateRawScreenPos();
        }
        else if(!usingCamera)
        {
            rawScreenPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        DampScreenPos();
    }

    void ParseData()
    {
        string new_data = "";
        if (data_queue.IsEmpty || !data_queue.TryDequeue(out new_data) || new_data.Equals(""))
        {
            return;
        }
        string[] vectors = new_data.Split('_');
        if (vectors.Length != 3)
        {
            Debug.LogError("Unexpected number of vectors recieved: " + vectors.Length);
            return;
        }
        Vector3 forward = ParseVector(vectors[0]);
        Vector3 up = ParseVector(vectors[1]);
        rot = Quaternion.LookRotation(new Vector3(forward.x, -forward.y, forward.z), new Vector3(up.x, -up.y, up.z));
        translation = ParseVector(vectors[2]);
    }

    Vector3 ParseVector(string vectorString)
    {
        string timmed_vector = vectorString.TrimStart('[').TrimEnd(']');
        string[] vector_elems = timmed_vector.Split(", ");
        if(vector_elems.Length != 3)
        {
            Debug.LogError("Malformed vector recieved. Expected 3 vector elements. Got: " + vector_elems.Length);
            return Vector3.zero;
        }
        Vector3 answer = Vector3.zero;
        answer.x = float.Parse(vector_elems[0], CultureInfo.InvariantCulture.NumberFormat);
        answer.y = float.Parse(vector_elems[1], CultureInfo.InvariantCulture.NumberFormat);
        answer.z = float.Parse(vector_elems[2], CultureInfo.InvariantCulture.NumberFormat);
        return answer;
    }
    private void UpdateRawScreenPos()
    {
        Vector3 intersection;

        bool isValid;

        (intersection, isValid) = GetIntersectionPoint(translation, rot * Vector3.forward);
        
        Debug.Log("Rvec: " + rot.eulerAngles + " tvec: " + translation + " insersection: " + intersection + " is valid: " + isValid);
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
        if(udpClientThread != null)
        {
            udpClientThread.Abort();
        }
        if(client != null)
        {
            client.Close();
            client = null;
        }
        
    }
}
