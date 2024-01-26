using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class DecalOpacityTween : MonoBehaviour
{
    public float targetOpacity = 0.0f;
    public float tweenDuration = 1.0f;

    private DecalProjector decalProjector;
    private Material decalMaterial;

    private void Start()
    {
        decalProjector = GetComponent<DecalProjector>();
        float rndRotation = Random.Range(1, 359);
        Vector3 newYRotation = new Vector3(90, 0, rndRotation);
        transform.rotation = Quaternion.Euler(newYRotation);

        //if (decalProjector == null)
        //{
        //    Debug.LogError("Decal Projector component not found!");
        //    return;
        //}

        //decalMaterial = decalProjector.material;

        //// Ensure the material has transparency enabled
        //EnsureMaterialHasTransparency();
        //FadeDecalOpacity();
    }

    private void EnsureMaterialHasTransparency()
    {
        if (decalMaterial != null && decalMaterial.HasProperty("_BaseColor"))
        {
            // Enable alpha blending for transparency
            decalMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            decalMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            // Enable alpha testing to handle semi-transparent pixels
            decalMaterial.EnableKeyword("_ALPHATEST_ON");
            decalMaterial.SetFloat("_AlphaCutoff", 0.5f); // Adjust as needed
        }
    }

    private void Update()
    {
        // Check for user input, for example, pressing a key to start the fade
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FadeDecalOpacity();
        }
    }

    private void FadeDecalOpacity()
    {
        if (decalMaterial != null)
        {
            // Use DOTween to tween the opacity
            decalMaterial.DOColor(new Color(decalMaterial.color.r, decalMaterial.color.g, decalMaterial.color.b, targetOpacity), tweenDuration);
        }
    }
}