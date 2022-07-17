using UnityEngine;

namespace Core.Spawnables
{
    /// <summary>
    /// Defines any item that can be instantiated on the map
    /// </summary>
    public class SpawnableItem : ScriptableObject
    {
        /// <summary>
        /// Name of the item to spawn
        /// </summary>
        public string itemName;

        /// <summary>
        /// The world prefab that will be used on instatiation
        /// </summary>
        public GameObject worldObjectPrefab;

    }
}
