using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyBullet : MonoBehaviour
{

    [SerializeField] private float speed = 2f;
    [SerializeField] private int playerDamage = 1;
    private Rigidbody m_Rigidbody;
    private bool isDestroyed;
    private Transform randomTarget;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        //Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
        GameObject[] getTargets = GameObject.FindGameObjectsWithTag("Player Target");
        randomTarget = getTargets[Random.Range(0, getTargets.Length)].transform;
        transform.LookAt(randomTarget);
        //transform.LookAt(new Vector3(pos.x, pos.y + 2.4f, pos.z));
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
    }

    public int GetPlayerDamage()
    {
        return playerDamage;
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }
}
