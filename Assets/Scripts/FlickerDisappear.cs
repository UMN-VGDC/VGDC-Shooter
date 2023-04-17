using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class FlickerDisappear : MonoBehaviour
{
    [SerializeField] private bool useSetActive;
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private int delay = 2500;
    [SerializeField] private int flickerAmount = 15;
    [SerializeField] private GameObject[] destroyImmediate;
    // Start is called before the first frame update
    void Start()
    {
        if (playOnAwake) Disappear();
    }

    public async void Disappear()
    {
        await UniTask.DelayFrame(delay);
        DestroyImmediate();
        var rend = GetComponentsInChildren<Renderer>();
        for (int r = 0; r < flickerAmount; r++)
        {
            Flicker(rend, false);
            await UniTask.DelayFrame(5);
            Flicker(rend, true);
            await UniTask.DelayFrame(5);
        }
        Destroy(gameObject);
    }

    private void Flicker(Renderer[] rend, bool state)
    {
        if (useSetActive) {
            gameObject.SetActive(state);
            return;
        }
        
        for (int i = 0; i < rend.Length; i++)
        {
            rend[i].enabled = state;
        }
    }

    private void DestroyImmediate()
    {
        for (int i = 0; i < destroyImmediate.Length; i++)
        {
            Destroy(destroyImmediate[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
