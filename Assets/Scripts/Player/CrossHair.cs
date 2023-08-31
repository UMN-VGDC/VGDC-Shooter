using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CrossHair : DamageEnemy
{
    [SerializeField] private LayerMask bulletLayerMask;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private GameObject critEffect;
    private Camera mainCamera;

    public static Action<Transform> raycastObject;
    private Transform hitTransform;
    private bool isDead;

    private void Start()
    {
        mainCamera = Camera.main;
        GameManager.hasDied += DisableCrossHair;
        GameManager.restartGame += EnableCrossHair;
    }

    private void DisableCrossHair() => isDead = true;

    private void EnableCrossHair() => isDead = false;

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        Vector3 screenPos = CameraReciever.instance.GetDampedScreenPos();
        Ray ray = mainCamera.ViewportPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000, ~bulletLayerMask))
        {
            aimTarget.position = hit.point;
            hitTransform = hit.transform;
            raycastObject?.Invoke(hit.transform);
        }

        transform.position = mainCamera.ViewportToWorldPoint(new Vector3(screenPos.x, screenPos.y, 1));

    }

    public void DecreaseEnemyHealth(int amount, int critAmount)
    {
        if (hitTransform == null) return;
        if (hitTransform.gameObject.tag == "Head" && hitTransform.root.tag == "Entity")
        {
            Instantiate(critEffect, aimTarget.position, Quaternion.identity);
        }
        EntityHit(hitTransform.gameObject, amount, critAmount);
    }
}
