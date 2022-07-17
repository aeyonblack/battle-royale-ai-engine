using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale.Level
{
    /// <summary>
    /// Session Manager handles session initialization and completion 
    /// </summary>
    public class SessionManager : MonoBehaviour
    {
        /// <summary>
        /// Current session being executed
        /// </summary>
        protected int currentIndex;

        /// <summary>
        /// Whether the session manager starts session on awake
        /// </summary>
        public bool startSessionOnAwake;

        /// <summary>
        /// The sessions to run in order
        /// </summary>
        [Tooltip("Specify this list in order")]
        public List<Session> sessions = new List<Session>();


        /// <summary>
        /// The current session number
        /// </summary>
        public int sessionNumber
        {
            get { return currentIndex + 1; }
        }

        /// <summary>
        /// Total number of sessions
        /// </summary>
        public int totalSessions
        {
            get { return sessions.Count; }
        }

        public float sessionProgress
        {
            get
            {
                if (sessions == null || sessions.Count <= currentIndex)
                {
                    return 0;
                }
                return sessions[currentIndex].progress;
            }
        }

        /// <summary>
        /// Fire when the session changes
        /// </summary>
        // public event Action sessionChanged;

        /// <summary>
        /// Called when all sessions are done
        /// </summary>
        public event Action spawnedCompleted;

        /// <summary>
        /// Starts the sessions
        /// </summary>
        public virtual void StartSessions()
        {

        }

        /// <summary>
        /// Initializes the first session
        /// </summary>
        protected virtual void Awake()
        {

        }

        /// <summary>
        /// Sets up the next session
        /// </summary>
        protected virtual void NextSession()
        {

        }

        /// <summary>
        /// Initialize the current session
        /// </summary>
        protected virtual void InitCurrentSession()
        {

        }

        protected virtual void SafetlyCallSpawningCompleted()
        {
            spawnedCompleted?.Invoke();
        }
    }
}
