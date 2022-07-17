using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the logic of a character's special ability
/// </summary>
public class Ability : ScriptableObject
{
    /// <summary>
    /// Name of the ability
    /// </summary>
    public string abilityName;

    /// <summary>
    /// How long the ability stays active (in seconds)
    /// </summary>
    public float activeTime;

    /// <summary>
    /// How long it takes for the ability to refresh (in seconds)
    /// </summary>
    public float refreshRate;

    /// <summary>
    /// Enables the ability
    /// </summary>
    /// <param name="parent">The character to which this ability belongs</param>
    public virtual void Activate(GameObject parent) {}
}
