using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyGunMover : MonoBehaviour {

    [SerializeField] private Transform ARCamera;
    [SerializeField] private Transform MainCamera;
    [SerializeField] private Transform reference;
    [SerializeField] private Transform viewportReference;
    [SerializeField] private Transform playerGunPos;
    [SerializeField] private Quaternion rotPreOffset;
    [SerializeField] private Quaternion rotPostOffset;
    [SerializeField] private float YOffset;
    [SerializeField] private float moveSensitivity = 1f;
    [SerializeField] private float lookSensitivity = 1f;
    [SerializeField] private float lookStabilization = 1f;


    void Update() {
        Vector3 pos = MainCamera.TransformPoint(ARCamera.InverseTransformPoint(reference.position));
        Vector3 gunPos = playerGunPos.transform.position;

        viewportReference.position = new Vector3(pos.x, pos.y + YOffset, gunPos.z);
        Vector3 posViewport = Camera.main.WorldToViewportPoint(viewportReference.position) + new Vector3(-0.5f, -0.5f, 0);
        posViewport = posViewport * moveSensitivity;

        transform.position = new Vector3(pos.x + posViewport.x, pos.y + posViewport.y + YOffset, gunPos.z);

        Vector3 lookAngles = (Quaternion.Inverse(ARCamera.rotation) * reference.rotation * rotPreOffset).eulerAngles;
        Quaternion lookRotation = Quaternion.Euler(lookAngles.x * lookSensitivity, lookAngles.y * lookSensitivity, lookAngles.z);
        Quaternion rotation = MainCamera.rotation * Quaternion.Euler(0, 0, 180) * lookRotation * rotPostOffset;

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * lookStabilization);
    }

}
