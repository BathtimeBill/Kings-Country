using System.Collections.Generic;
using UnityEngine;

public class SnowTree : GameBehaviour
{
    public List<GameObject> meshes;
    public Renderer[] treeRenderers;
    public Material summerFoilage;
    public Material winterFoilage;

    void Start()
    {   /*int meshNo = meshes.Length;*/
        foreach (var mesh in meshes)
        {
            //treeRenderers.AddRange(mesh.GetComponentsInChildren<Renderer>());
        }

        foreach (Renderer renderer in treeRenderers)
        {
            int i = renderer.materials.Length;
            renderer.materials[i] = winterFoilage;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
