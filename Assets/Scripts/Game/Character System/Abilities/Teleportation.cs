using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{

    public FixedButton TeleportButton;
    public GameObject[] Root;
    public GameObject SpawnEffect;

    private void Start()
    {
        TeleportButton.ButtonPressed += () =>
        {
            StartCoroutine(Teleport());
        };
    }

    private IEnumerator Teleport()
    {
        Helpers.RecursiveLayerChange(transform.parent, LayerMask.NameToLayer("Invisible"));

        yield return ToggleCharacter(false );

        yield return new WaitForSeconds(2f);

        Vector3 newPos = transform.position + Random.insideUnitSphere * 10f;
        newPos.y = transform.position.y;
        transform.position = newPos;
        
        yield return ToggleCharacter(true);

        Helpers.RecursiveLayerChange(transform.parent, LayerMask.NameToLayer("Character"));
    }

    private IEnumerator ToggleCharacter(bool visible)
    {
        var effect = Poolable.TryGetPoolable<Poolable>(SpawnEffect);
        effect.transform.position = transform.position; 
        yield return new WaitForSeconds(visible ? 0.2f : 0.15f);
        foreach (var obj in Root)
        {
            obj.SetActive(visible);
        }
    }

}
