using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteFade : MonoBehaviour
{
    [SerializeField] private new Renderer renderer;
    [SerializeField] private float swayAmount = 10;
    [SerializeField] private float swaySpeed = 1.5f;
    [SerializeField] private float descendSpeed = 0.5f;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, swayAmount * 0.5f);
        float delay = Random.Range(0f, swaySpeed);
        transform.DORotate(new Vector3(0f, 0f, -swayAmount), swaySpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
    }
    public void FadeParachute()
    {
        DOVirtual.Float(0, 1, 2f, e =>
        {
            renderer.material.SetFloat("_Alpha_Clip", e);
        }).OnComplete(() => Destroy(gameObject));
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y - descendSpeed * Time.deltaTime, pos.z);
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}
