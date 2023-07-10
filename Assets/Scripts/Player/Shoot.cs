using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using System.Runtime.InteropServices;

public class Shoot : MonoBehaviour
{
    public enum AimType
    {
        Mouse,
        Wiimote
    }
    public AimType aimtype;
    private Vector3 aimTransform;

    [SerializeField] private Transform raycastObject;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private Transform spawner;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int fireRate = 30;
    [SerializeField] private LayerMask bulletLayerMask;

    [SerializeField] private GameObject recoilObject;
    [SerializeField] private float recoilStrength = 5f;

    [SerializeField] private ParticleSystem muzzleFlash;

    [DllImport("user32.dll")] static extern bool SetCursorPos(int x, int y);
    [SerializeField] private Transform wiimoteAimTransform;

    private float _currentFireCountdown = 1f;
    private bool isShooting;

    public static event Action shootBullet;

    // Start is called before the first frame update
    void Start()
    {
        isShooting = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (aimtype)
        {
            case AimType.Mouse:
                aimTransform = Input.mousePosition;
                break;
            case AimType.Wiimote:
                aimTransform = wiimoteAimTransform.position;
                break;

        }

        raycastObject.position = aimTransform;
        Ray ray = Camera.main.ScreenPointToRay(raycastObject.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000, ~bulletLayerMask))
        {
            crossHair.transform.position = hit.point;
            transform.LookAt(hit.point);
        }

        if (isShooting) ShootBullet();

    }

    private void ShootBullet()
    {
        _currentFireCountdown = Mathf.MoveTowards(_currentFireCountdown, 0f, Time.deltaTime * fireRate);
        if (_currentFireCountdown <= 0f)
        {
            Instantiate(bullet, spawner.position, transform.rotation);
            Recoil();
            muzzleFlash.Play();
            shootBullet?.Invoke();
            _currentFireCountdown = 1f;
        }
    }

    private void Recoil()
    {
        recoilObject.transform.localPosition = new Vector3(0, 0, 0);
        recoilObject.transform.DOLocalMoveZ(0f - (0.1f * recoilStrength), 0.01f);
    }
}
