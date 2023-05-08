using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBloodScratchDecal : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.DOColor(new Color(0, 0, 0, 0), fadeDuration).SetDelay(0.5f).onComplete = () => Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
