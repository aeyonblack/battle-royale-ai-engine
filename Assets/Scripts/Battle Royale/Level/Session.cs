using Core.Extensions;
using Core.Spawnables;
using Core.Utilities;
using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;


namespace BattleRoyale.Level
{
    /// <summary>
    /// A session is a TimedBehaviour, that uses the RepeatingTimer to spawn groups of items
    /// </summary>
    public class Session : TimedBehaviour
    {
        /// <summary>
        /// A library of items to be spawned
        /// </summary>
        public List<SpawnableItem> spawnableItems;

        /// <summary>
        /// A collection of all possible points players can spawn at
        /// </summary>
        public List<Transform> hotspots;

        /// <summary>
        /// A list of instructions on how spawnable items in the 
        /// session's library are spawned
        /// </summary>
        public List<SpawnInstructions> spawnInstructions { get; protected set; } = new List<SpawnInstructions>();

        /// <summary>
        /// The index of the current item to spawn 
        /// </summary>
        protected int currentIndex;

        /// <summary>
        /// The number of items to still
        /// </summary>
        public int numberOfItemsToSpawn;

        /// <summary>
        /// The delay between the current and the next spawn
        /// </summary>
        public float delayToSpawn;

        /// <summary>
        /// The repeating timer used to spawn items
        /// </summary>
        protected RepeatingTimer spawnTimer;

        protected SpawnType spawnType = SpawnType.CHARACTER;

        /// <summary>
        /// The event that is fired when a session is completed
        /// </summary>
        public event Action sessionCompleted; 

        public virtual float progress
        {
            get { return (float)(currentIndex / spawnInstructions.Count); }
        }

        /// <summary>
        /// Initialize the session
        /// </summary>
        public virtual void Init()
        {
            if (spawnInstructions.Count == 0)
            {
                CreateSpawnInstructions();
            }
            spawnTimer = new RepeatingTimer(spawnInstructions[0].delayToSpawn, SpawnCurrent);
            StartTimer(spawnTimer);
        }

        /// <summary>
        /// Create randomized spawn instructions
        /// </summary>
        private void CreateSpawnInstructions()
        {
            for (int i = 0; i < numberOfItemsToSpawn; i++)
            {
                SpawnInstructions instruction = new SpawnInstructions();
                instruction.delayToSpawn = delayToSpawn;
                instruction.spawnable = spawnableItems[Random.Range(0, spawnableItems.Count)];
                spawnInstructions.Add(instruction);
            }
        }

        /// <summary>
        /// Handles spawning the current item and sets up the next item for spawning
        /// </summary>
        protected virtual void SpawnCurrent()
        {
            Spawn();
            if (!TrySetupNextSpawn())
            {
                SafelyBroadcastSessionCompleted();
                currentIndex = spawnInstructions.Count;
                StopTimer(spawnTimer);
            }
        }

        /// <summary>
        /// Spawns the current item
        /// </summary>
        protected void Spawn()
        {
            SpawnInstructions instruction = spawnInstructions[currentIndex];
            SpawnItem(instruction.spawnable);
        }

        /// <summary>
        /// Tries to setup the next spawn
        /// </summary>
        /// <returns>True if there's another spawn instruction, false otherwise</returns>
        protected bool TrySetupNextSpawn()
        {
            bool hasNext = spawnInstructions.Next(ref currentIndex);
            if (hasNext)
            {
                SpawnInstructions nextInstruction = spawnInstructions[currentIndex];
                if(nextInstruction.delayToSpawn <= 0)
                {
                    SpawnCurrent();
                }
                else
                {
                    spawnTimer.SetTime(nextInstruction.delayToSpawn);
                }
            }
            return hasNext;
        }

        /// <summary>
        /// Spawns the item
        /// </summary>
        /// <param name="item">The item to spawn</param>
        protected virtual void SpawnItem(SpawnableItem item)
        {
            Vector3 spawnPosition = spawnType == SpawnType.CHARACTER ?
                hotspots[Random.Range(0, hotspots.Count)].position : SpawnHotspot.CreateNew();

            Instantiate(item.worldObjectPrefab, spawnPosition, Quaternion.identity);
        }

        protected void SafelyBroadcastSessionCompleted()
        {
            sessionCompleted?.Invoke();
        }

    }
}
