using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject[] bloodScratch;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.damageTaken += spawnBloodScratch;
    }

    private void spawnBloodScratch()
    {
        Vector2 ramdomPos = new Vector2(Random.Range(-200f, 200f), Random.Range(-150f, 50f));
        GameObject decal = bloodScratch[Random.Range(0, bloodScratch.Length)];
        GameObject instantiatedDecal = Instantiate(decal, Vector2.zero, Quaternion.identity, canvas.transform);
        RectTransform rt = instantiatedDecal.GetComponent<RectTransform>();
        rt.anchoredPosition = rt.transform.position;
        rt.localPosition = ramdomPos;

        int flip = Random.value < 0.5f ? 1 : -1;
        Vector2 initialScale = instantiatedDecal.transform.localScale;
        instantiatedDecal.transform.localScale = new Vector2(initialScale.x * flip, initialScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        PlayerHealth.damageTaken -= spawnBloodScratch;
    }
}
