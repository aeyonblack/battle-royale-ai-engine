using System.Collections;
using System;
using UnityEngine;

[CreateAssetMenu(fileName ="Telekinesis", 
    menuName = "Epic Legends/Create/Ability/Telekinesis")]
public class Telekinesis : Ability
{
    public float effectRadius;

    public override void Activate(GameObject parent)
    {
        Collider[] colliders = Physics.OverlapSphere(parent.transform.position, effectRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Character") // let's make this more accurate
                && !isEqual(parent, hit))
            {
                //parent.GetComponent<TelekineticFreeze>().Freeze(hit, activeTime);
            }
        }
    }

    private bool isEqual(GameObject parent, Collider target)
    {
        bool equal = false;
        var character = target.GetComponent<CharacterData>();
        if (character == null)
        {
            if (target.transform.parent.name == parent.name) equal = true;
        } 
        else
        {
            if (target.name == parent.name) equal = true;
        }
        return equal;
    }

}
