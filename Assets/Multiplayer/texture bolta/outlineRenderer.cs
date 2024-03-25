using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class outlineRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Material outlienMaterial;
    [SerializeField] private float outlineScalefactor;
    [SerializeField] private Color outlineColor;
    private Renderer outlieRenderer;

   

  void Start()
    {

        outlieRenderer = CreateOutline(outlienMaterial, outlineScalefactor, outlineColor);
        outlieRenderer.enabled = true;
    }

    Renderer CreateOutline(Material outlineMat,float ScaleFactor,Color color)
    {
        GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        Renderer rend=outlineObject.GetComponent<Renderer>();

        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", color);
        rend.material.SetFloat("Scale", ScaleFactor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        outlineObject.GetComponent<outlineRenderer>().enabled = false;
        outlineObject.GetComponent<Collider>().enabled = false;

        rend.enabled = false;

        return rend;

    }

   




}
