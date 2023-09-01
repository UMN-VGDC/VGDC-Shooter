using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Bullet : DamageEnemy
{

    [SerializeField] private float speed = 2f;
    [SerializeField] private GameObject critEffect;
    [SerializeField] private GameObject bulletImpactEffect;
    [SerializeField] private int deleteTime = 300;
    [SerializeField] private Transform predictionTransform;
    [SerializeField] private int Damage = 1, Crit = 2;

    Rigidbody m_Rigidbody;
    private bool isDestroyed;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        DestroyTimer();
        StartCoroutine(Predict());
    }

    private async void DestroyTimer()
    {
        await UniTask.DelayFrame(deleteTime);
        if (isDestroyed) return;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        m_Rigidbody.MovePosition(pos + (transform.forward * 0.1f * speed));
        StartCoroutine(Predict());
        transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(20, 20, 20), Time.deltaTime * 8);
    }

    IEnumerator Predict()
    {
        Vector3 prediction = transform.position + m_Rigidbody.velocity * Time.fixedDeltaTime;
        RaycastHit hit2;
        int layerMask =~ LayerMask.GetMask("Bullet");
        Debug.DrawLine(predictionTransform.position, prediction, Color.red, 2.5f);
        if(Physics.Linecast(predictionTransform.position, prediction, out hit2, layerMask))
        {
            transform.position = hit2.point;
            m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            m_Rigidbody.isKinematic = true;
            yield return 0;
            OnTriggerEnterFixed();
            if (hit2.collider == null) yield break;
            EntityHit(hit2.collider.gameObject, Damage, Crit);
            if (hit2.collider.gameObject.tag == "Head" && hit2.transform.root.tag == "Entity")
            {
                Instantiate(critEffect, hit2.point, Quaternion.identity);
            }
            Instantiate(bulletImpactEffect, hit2.point, Quaternion.identity);
        }
    }

    void OnTriggerEnterFixed()
    {
       Destroy(gameObject); 
    }

    void OnDestroy()
    {
        isDestroyed = true;
    }
}
