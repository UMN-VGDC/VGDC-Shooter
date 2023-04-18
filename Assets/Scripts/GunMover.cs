using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMover : MonoBehaviour {

    [SerializeField] private Transform ARCamera;
    [SerializeField] private Transform MainCamera;
    [SerializeField] private Transform reference;
    [SerializeField] private Quaternion rotationoffset;
    [SerializeField] private float ZOffset;
    [SerializeField] private float YOffset;

    void Update() {
        Vector3 pos = MainCamera.TransformPoint(ARCamera.InverseTransformPoint(reference.position));
        transform.position = new Vector3(pos.x, pos.y + YOffset, ZOffset);
        transform.rotation = MainCamera.rotation * Quaternion.Euler(0, 0, 180) * Quaternion.Inverse(ARCamera.rotation) * reference.rotation * rotationoffset;
    }

}
