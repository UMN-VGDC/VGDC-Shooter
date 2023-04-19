using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMover : MonoBehaviour {

    [SerializeField] private Transform ARCamera;
    [SerializeField] private Transform MainCamera;
    [SerializeField] private Transform reference;
    [SerializeField] private Transform viewportReference;
    [SerializeField] private Quaternion rotationOffset;
    [SerializeField] private float ZOffset;
    [SerializeField] private float YOffset;
    [SerializeField] private float moveSensitivity = 1f;


    void Update() {
        Vector3 pos = MainCamera.TransformPoint(ARCamera.InverseTransformPoint(reference.position));

        viewportReference.position = new Vector3(pos.x, pos.y + YOffset, ZOffset);
        Vector3 posViewport = Camera.main.WorldToViewportPoint(viewportReference.position) + new Vector3(-0.5f, -0.5f, 0);
        posViewport = posViewport * moveSensitivity;

        transform.position = new Vector3(pos.x + posViewport.x, pos.y + posViewport.y + YOffset, ZOffset);

        transform.rotation = MainCamera.rotation * Quaternion.Euler(0, 0, 180) * Quaternion.Inverse(ARCamera.rotation) * reference.rotation * rotationOffset;
    }

}
