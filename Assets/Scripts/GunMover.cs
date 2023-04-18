using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMover : MonoBehaviour {

    public Transform ARCamera;
    public Transform MainCamera;
    public Transform reference;

    void Update() {
        transform.position = MainCamera.TransformPoint(ARCamera.InverseTransformPoint(reference.position));
        transform.rotation = MainCamera.rotation * Quaternion.Euler(0, 0, 180) * Quaternion.Inverse(ARCamera.rotation) * reference.rotation;
    }

}
