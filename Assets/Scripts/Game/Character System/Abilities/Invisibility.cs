using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisibility : MonoBehaviour
{
    public FixedButton AbilityButton;
    public float duration;
    public GameObject Body;
    public GameObject Head;
    public Material HologramMat;

    private Material defaultBodyMaterial;
    private Material defaultHeadMaterial;
    private SkinnedMeshRenderer bodyMeshRenderer;
    private SkinnedMeshRenderer headMeshRenderer;

    private void Start()
    {
        bodyMeshRenderer = Body.GetComponent<SkinnedMeshRenderer>();
        headMeshRenderer = Head.GetComponent<SkinnedMeshRenderer>();
        defaultBodyMaterial = bodyMeshRenderer.material;
        defaultHeadMaterial = headMeshRenderer.material;
        AbilityButton.ButtonPressed += () => StartCoroutine(BecomeInvisible());
    }

    private IEnumerator BecomeInvisible()
    {
        bodyMeshRenderer.material = HologramMat;
        headMeshRenderer.material = HologramMat;
        Helpers.RecursiveLayerChange(transform, LayerMask.NameToLayer("Invisible"));
        yield return new WaitForSeconds(duration);
        bodyMeshRenderer.material = defaultBodyMaterial;
        headMeshRenderer.material = defaultHeadMaterial;
        Helpers.RecursiveLayerChange(transform, LayerMask.NameToLayer("Character"));
    }
}
