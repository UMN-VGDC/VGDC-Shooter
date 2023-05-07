using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LegacyShoot : MonoBehaviour
{
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireRate = 10f;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private LayerMask bulletLayerMask;

    [SerializeField] private GameObject recoilObject;
    [SerializeField] private float recoilStrength = 5f;
    
    private float _currentFireCountdown = 1f;
    private bool isShooting;
    private Vector3 initialZPos;

    // Start is called before the first frame update
    void Start()
    {
        isShooting = true;
        initialZPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            FireRay();
            ShootBullet();
        }
    }

    void FireRay()
    {
        Ray ray = new Ray(spawner.transform.position, spawner.transform.forward);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData, 1000f, ~bulletLayerMask);
        crossHair.transform.position = hitData.point;
    }

    private void ShootBullet()
    {
        _currentFireCountdown = Mathf.MoveTowards(_currentFireCountdown, 0f, Time.deltaTime * fireRate);
        if (_currentFireCountdown <= 0f)
        {
            Instantiate(bullet, spawner.transform.position, transform.rotation);
            Recoil();
            _currentFireCountdown = 1f;
        }
    }

    private void Recoil()
    {
        // transform.DOMove(new Vector3(pos.x, pos.y, pos.z - (0.01f * recoilStrength)), 0.02f);
        recoilObject.transform.localPosition = new Vector3(0, 0, 0); 
        recoilObject.transform.DOLocalMoveZ(0f - (0.1f * recoilStrength), 0.01f);
    }


}
