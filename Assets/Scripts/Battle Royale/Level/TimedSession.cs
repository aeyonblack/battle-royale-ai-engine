namespace BattleRoyale.Level
{
    /// <summary>
    /// A Session implementation that triggers the sessionCompleted
    /// event after an elapsed amount of time
    /// </summary>
    public class TimedSession : Session
    {

        /// <summary>
        /// The time until the next session is started
        /// </summary>
        public float timeToNextSession = 10f;

        /// <summary>
        /// The timer used to start the next session
        /// </summary>
        protected Timer sessionTimer;

        public override float progress
        {
            get { return sessionTimer == null ? 0 : sessionTimer.normalizedProgress; }
        }

        /// <summary>
        /// Initializes the session
        /// </summary>
        public override void Init()
        {
            base.Init();

            if (spawnInstructions.Count > 0)
            {
                sessionTimer = new Timer(timeToNextSession, SafelyBroadcastSessionCompleted);
                StartTimer(sessionTimer);
            }
        }

        protected override void SpawnCurrent()
        {
            
        }

    }
}
