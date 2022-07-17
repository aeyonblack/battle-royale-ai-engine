using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="medkit", menuName = "Epic Legends/Create/Medkit")]
public class Medkit : Collectable
{
    public float amountBoost;

    /// <summary>
    /// Try to use the loot item here
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public override bool UsedBy(CharacterData user)
    {
        if (user.health.currentHealth == user.health.maxHealth)
            return false;
        user.health.ModifyHealth(amountBoost);
        user.HealingFeedbacks?.PlayFeedbacks();
        return true;
    }

}
