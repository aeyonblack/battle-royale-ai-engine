using Core.Spawnables;
using System;

namespace BattleRoyale.Level
{
    /// <summary>
    /// Serializable class for specifying properties of spawning an item
    /// </summary>
    [Serializable]
    public class SpawnInstructions {

        /// <summary>
        /// The item to spawn 
        /// </summary>
        public SpawnableItem spawnable;

        /// <summary>
        /// The delay from the previous spawn until when this spawnable is spawned
        /// </summary>
        public float delayToSpawn;

    }
}
