using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CrossHair : DamageEnemy
{
    [SerializeField] private LayerMask bulletLayerMask;
    [SerializeField] private Transform aimTarget;
    private Camera mainCamera;

    public static Action<Transform> raycastObject;
    private Transform hitTransform;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000, ~bulletLayerMask))
        {
            aimTarget.position = hit.point;
            hitTransform = hit.transform;
            raycastObject?.Invoke(hit.transform);
        }

        transform.position = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 1));

    }

    public void DecreaseEnemyHealth(int amount, int critAmount)
    {
        EntityHit(hitTransform.gameObject, amount, critAmount);
    }
}
