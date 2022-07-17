using System;
using UnityEngine;


namespace BattleRoyale.Level
{
    /// <summary>
    /// The Level Manager handles level state and tracks player trophies
    /// </summary>
    [RequireComponent(typeof(SessionManager))]
    public class LevelManager : Singleton<LevelManager>
    {
        /// <summary>
        /// Number of players currently in the level
        /// </summary>
        public float numberOfPlayers { get; protected set; }

        /// <summary>
        /// The attached session manager
        /// </summary>
        public SessionManager sessionManager { get; protected set; }

        /// <summary>
        /// The current state of the level 
        /// </summary>
        public LevelState levelState { get; protected set; }

        /// <summary>
        /// Invoked when the number of players in the level changes
        /// </summary>
        public event Action numberOfPlayersChanged;

        /// <summary>
        /// Invoked when the main player is killed
        /// </summary>
        // public event Action playerKilled;

        /// <summary>
        /// Invoked when the level is complete
        /// </summary>
        public event Action levelCompleted;

        /// <summary>
        /// Invoked when the level is failed
        /// </summary>
        public event Action levelFailed;

        /// <summary>
        /// The time in seconds the level is meant to last for
        /// </summary>
        public float levelTime;

        /// <summary>
        /// Keeps track of the remaining time before the level is over
        /// </summary>
        private float timeLeft;

        [Header("Map Bounds")]
        public float xMin;
        public float xMax;
        public float zMin;
        public float zMax;

        protected override void Awake()
        {
            base.Awake();
            numberOfPlayers = GameObject.FindGameObjectsWithTag("Legend").Length + 1; // change this ASAP
            timeLeft = levelTime;
        }

        private void Update()
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Game over, time ran out");
            }
        }

        /// <summary>
        /// Increments the number of players on agent/player spawn
        /// </summary>
        public virtual void IncrementNumberOfPlayers()
        {
            numberOfPlayers++;
        }

        /// <summary>
        /// Decrements the number of players on agent/player death
        /// </summary>
        public virtual void DecrementNumberOfPlayers()
        {
            numberOfPlayers--;
            if (numberOfPlayers < 0)
            {
                Debug.LogError("The number of enemies is negative, fix this!");
            }

            if (numberOfPlayers == 1)
            {
                Debug.Log("Game over, one player remaining");
            }
        }

        /// <summary>
        /// Fired when the session manager is done spawning
        /// </summary>
        protected virtual void OnSpawningCompleted()
        {

        }

        /// <summary>
        /// Changes the level state and broadcasts the event
        /// </summary>
        /// <param name="newState">The new state to transition to</param>
        protected virtual void ChangeLevelState(LevelState newState)
        {

        }

        /// <summary>
        /// Calls the level complete event
        /// </summary>
        protected virtual void SafelyCallLevelComplete()
        {
            levelCompleted?.Invoke();
        }

        /// <summary>
        /// Calls the numberOfPlayersChanged event
        /// </summary>
        protected virtual void SafelyCallNumberOfPlayersChanged()
        {
            numberOfPlayersChanged?.Invoke();
        }

        protected virtual void SafelyCallLevelFailed()
        {
            levelFailed?.Invoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}
