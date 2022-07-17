using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="radioactive gas", 
    menuName = "Epic Legends/Create/Ability/Radioactive Gas")]
public class RadioactiveGas : Ability
{
    public override void Activate(GameObject parent)
    {
        Debug.Log(parent.name + " " + abilityName);
        Debug.Log("-----SPRAYED RADIOACTIVE GAS------");
    }
}
