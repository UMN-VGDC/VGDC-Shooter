using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float fireRate = 10f;
    [SerializeField] private float bulletSpeed = 10f;

    [SerializeField] private float recoilStrength = 5f;
    
    private float _currentFireCountdown = 1f;
    private bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        isShooting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            ShootBullet();
        }
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
        Vector3 pos = transform.position;
        transform.DOMove(new Vector3(pos.x, pos.y, pos.z - (0.01f * recoilStrength)), 0.05f);
    }

    public float getBulletSpeed()
    {
        return bulletSpeed;
    }

}
