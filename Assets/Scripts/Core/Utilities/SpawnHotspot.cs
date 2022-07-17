using BattleRoyale.Level;
using UnityEngine;

namespace Core.Utilities
{
    /// <summary>
    /// Handles the creation of a random spawn hotspot bounded by the map
    /// </summary>
    public class SpawnHotspot
    {
        /// <summary>
        /// A reference to the LevelManager instance
        /// </summary>
        private static LevelManager manager = LevelManager.instance;

        /// <summary>
        /// Selects a suitable spawn hotspot and returns it
        /// </summary>
        /// <returns>A position vector</returns>
        public static Vector3 CreateNew()
        {
            while (true)
            {
                Vector3 hotspot = GenerateHotspot();
                Collider[] colliders = Physics.OverlapSphere(hotspot, 2f);
                if (colliders.Length == 0) return hotspot;
            }
        }

        /// <summary>
        /// Generates a possible hotspot [transform.position] within the map
        /// </summary>
        /// <returns>A position vector</returns>
        private static Vector3 GenerateHotspot()
        {
            if (manager)
            {
                float x = Random.Range(manager.xMin, manager.xMax);
                float z = Random.Range(manager.zMin, manager.zMax);
                return new Vector3(x, 1.5f, z);
            }
            return Vector3.zero;
        }
    }
}
