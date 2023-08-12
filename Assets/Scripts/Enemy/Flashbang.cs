using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Flashbang : MonoBehaviour
{
    [SerializeField] private Transform lookAtTransform;
    [SerializeField] private float turnspeed = 100f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Transform grenadeTransform;
    [SerializeField] private GameObject flash;
    [SerializeField] private int flashCount = 15;
    [SerializeField] private AudioClip beepSound;
    private Transform playerPos;
    private bool isMoving = true;

    private AudioSource audioSource;
    public static int count;
    public static Action flashbangExplode;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] getTargets = GameObject.FindGameObjectsWithTag("Player Target");
        playerPos = getTargets[UnityEngine.Random.Range(0, getTargets.Length)].transform;
        grenadeTransform.DOLocalRotate(new Vector3(180, 0, 0), 1f).SetLoops(-1, LoopType.Yoyo);
        audioSource = GetComponent<AudioSource>();
        flash.SetActive(false);
        FlashTImer();
        count++;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving) return;
        lookAtTransform.LookAt(playerPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtTransform.rotation, Time.deltaTime * turnspeed);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Attack Radius") return;
        isMoving = false;
        transform.SetParent(other.transform);
    }

    private float timer = 500;
    private int currentFlashCount;
    private bool isDestroyed;
    private async void FlashTImer()
    {
        flash.SetActive(true);
        audioSource.PlayOneShot(beepSound);
        await Task.Delay(Mathf.RoundToInt(timer));

        if (isDestroyed) return;
        flash.SetActive(false);
        await Task.Delay(Mathf.RoundToInt(timer));

        if (isDestroyed) return;
        timer *= 0.8f;
        audioSource.pitch += 0.05f;
        currentFlashCount++;
        if (currentFlashCount == flashCount)
        {
            GetComponent<EntityHealth>().DecreaseHealth(20);
            flashbangExplode?.Invoke();
            return;
        }
        FlashTImer();
    }

    private void OnDestroy()
    {
        DOTween.Kill(grenadeTransform);
        isDestroyed = true;
        count--;
    }

}
