using Core.Spawnables;

public abstract class Collectable : SpawnableItem
{
    /// <summary>
    /// Determines if the consumable depletes when it's used
    /// </summary>
    public bool consumable;

    /// <summary>
    /// Determines whether the item can be stacked or not
    /// </summary>
    public bool isStackable;

    /// <summary>
    /// Defines how a specific item is used and by who
    /// </summary>
    /// <param name="user">The character that used the item, could be the player
    /// or an agent</param>
    /// <returns>true if the item is used</returns>
    public abstract bool UsedBy(CharacterData user);

}
