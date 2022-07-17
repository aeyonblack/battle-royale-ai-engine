using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AbilityActiveState
{
    READY,
    REFRESHING,
    ACTIVE
}

public class CharacterAbilitiesHandler : MonoBehaviour
{
    /// <summary>
    /// Each button is mapped to an ability with the same index
    /// </summary>
    public FixedButton[] abilityButtons;

    /// <summary>
    /// A list of the character's abilities
    /// </summary>
    public Ability[] abilities;

    /// <summary>
    /// How long the ability is going to be active for
    /// </summary>
    public float activeTime;

    /// <summary>
    ///  How long the ability will take to refresh
    /// </summary>
    public float refreshRate;

    /// <summary>
    ///  The current state of the ability
    /// </summary>
    // private AbilityActiveState state = AbilityActiveState.READY;

    private FixedButton defaultAbility;
    private FixedButton alternateAbility;

    private void Start()
    {
        // setup the abilities here
        defaultAbility = abilityButtons[0];
        alternateAbility = abilityButtons.Length > 1 ? abilityButtons[1] : null;

        defaultAbility.ButtonPressed += ActivateDefaultAbility;

        if (alternateAbility) alternateAbility.ButtonPressed += ActivateAlternateAbility;
    }


    private void ActivateDefaultAbility()
    {
        abilities[0].Activate(gameObject);
    }

    private void ActivateAlternateAbility()
    {
        abilities[1].Activate(gameObject);
    }

}
