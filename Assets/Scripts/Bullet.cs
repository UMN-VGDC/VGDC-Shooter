using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Bullet : MonoBehaviour
{

    [SerializeField] private Shoot shoot;
    [SerializeField] private float speed = 2f;
    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor;

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
            EntityHitFlash(hit2.collider.gameObject);
        }
    }

    void OnTriggerEnterFixed()
    {
       Destroy(gameObject); 
    }

    async void EntityHitFlash(GameObject entity)
    {   
        var renderer = entity.transform.root.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderer.Length; i++)
        {
            SetRenderColor(renderer[i], flashColor);
        }
        await UniTask.DelayFrame(10);
        for (int i = 0; i < renderer.Length; i++)
        {
            SetRenderColor(renderer[i], new Color(1, 1, 1, 1));
        }
        
    }

    private void SetRenderColor(Renderer rend, Color color)
    {
        for (int i = 0; i < rend.materials.Length; i++)
        {
            rend.materials[i].SetColor("_Hit_Flash_1", color);
        }
    }

    void OnDestroy()
    {
        isDestroyed = true;
    }
}
