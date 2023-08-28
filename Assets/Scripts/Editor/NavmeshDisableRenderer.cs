using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;
using UnityEditor;
using System.Threading.Tasks;

[CustomEditor(typeof(NavmeshDisableRendererButton))]
public class NavmeshDisableRenderer : Editor
{
    public async override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NavmeshDisableRendererButton NMbutton = (NavmeshDisableRendererButton)target;
        if (GUILayout.Button("Generate Navmesh"))
        {
            NMbutton.EnableRenderers(true);
            NavMeshBuilder.ClearAllNavMeshes();
            NavMeshBuilder.BuildNavMeshAsync();
            while (NavMeshBuilder.isRunning)
            {
                await Task.Yield();
            }
            Debug.Log("Navmesh Successfully baked!");
            NMbutton.EnableRenderers(false);
        }
    }
}
