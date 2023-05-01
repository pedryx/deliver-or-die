namespace DeliverOrDie.Components;
/// <summary>
/// Time to live component. Entities with this component will get destroyed after <see cref="Time"/> reaches zero.
/// </summary>
internal struct TimeToLive
{
    /// <summary>
    /// Reaming time until entity is destroyed.
    /// </summary>
    public float Time;
    public int EntityIndex;

    public TimeToLive(int entityIndex, float time)
    {
        EntityIndex = entityIndex;
        Time = time;
    }
}
