using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed = 2f;
    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor;
    [SerializeField] private GameObject critEffect;
    [SerializeField] private GameObject bulletImpactEffect;

    public static Action crit;

    Rigidbody m_Rigidbody;
    private bool isDestroyed;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        DestroyTimer();
    }

    private async void DestroyTimer()
    {
        await UniTask.DelayFrame(300);
        if (isDestroyed) return;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        m_Rigidbody.MovePosition(pos + (transform.forward * 0.1f * speed));
        StartCoroutine(Predict());
    }

    IEnumerator Predict()
    {
        Vector3 prediction = transform.position + m_Rigidbody.velocity * Time.fixedDeltaTime;
        RaycastHit hit2;
        int layerMask =~ LayerMask.GetMask("Bullet");
        //Debug.DrawLine(transform.position, prediction);
        if(Physics.Linecast(transform.position, prediction, out hit2, layerMask))
        {
            transform.position = hit2.point;
            m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            m_Rigidbody.isKinematic = true;
            yield return 0;
            OnTriggerEnterFixed();
            EntityHit(hit2.collider.gameObject);
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

    void EntityHit(GameObject entity)
    {   
        var root = entity.transform.root;
        if (entity.tag == "Head" && root.tag == "Entity")
        {
            root.GetComponent<EntityHealth>().DecreaseHealth(2);
            crit?.Invoke();
            return;
        }

        if (root.tag == "Entity")
        {
            root.GetComponent<EntityHealth>().DecreaseHealth(1);
            return;
        }

        if (entity.tag == "Entity")
        {
            entity.GetComponent<EntityHealth>().DecreaseHealth(1);
        }

    }

    void OnDestroy()
    {
        isDestroyed = true;
    }
}
