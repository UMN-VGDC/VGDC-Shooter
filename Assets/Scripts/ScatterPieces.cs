using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ScatterPieces : MonoBehaviour
{

    private Rigidbody[] deathRB;
    [SerializeField] private float forceStrength = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        deathRB = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < deathRB.Length; i++) 
        {
            deathRB[i].AddForce(Random.insideUnitSphere * forceStrength);
        }
        FlickerDisappear();
    }

    private async void FlickerDisappear()
    {
        await UniTask.DelayFrame(2500);
        var rend = GetComponentsInChildren<Renderer>();
        for (int r = 0; r < 15; r++)
        {
            for (int i = 0; i < rend.Length; i++)
            {
                rend[i].enabled = false;
            }
            await UniTask.DelayFrame(5);
            for (int i = 0; i < rend.Length; i++)
            {
                rend[i].enabled = true;
            }
            await UniTask.DelayFrame(5);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
