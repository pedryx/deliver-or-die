namespace DeliverOrDie.Components;

internal struct ZombieBehavior
{
    /// <summary>
    /// Move speed of zombies in pixels per second.
    /// </summary>
    public float MoveSpeed;
    /// <summary>
    /// Current behavior state.
    /// </summary>
    public BehaviorState State;
    /// <summary>
    /// How many seconds it takes for zombie to attack. Zombie deal damage at the start of attack.
    /// </summary>
    public float AttackDuration;
    /// <summary>
    /// How many seconds elapsed from zombie's last action.
    /// </summary>
    public float Elapsed;

    /// <summary>
    /// Represent stages of zombie behavior.
    /// </summary>
    public enum BehaviorState
    {
        /// <summary>
        /// Zombie is idling.
        /// </summary>
        Idle,
        /// <summary>
        /// Zombie is moving towards the player.
        /// </summary>
        Move,
        /// <summary>
        /// Zombie is attacking player.
        /// </summary>
        Attack,
    }
}
