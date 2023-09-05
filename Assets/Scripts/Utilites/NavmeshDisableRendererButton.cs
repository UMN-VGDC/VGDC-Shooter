using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NavmeshDisableRendererButton : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;

    public void EnableRenderers(bool b)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = b;
        }
    }
}
