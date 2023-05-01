using DeliverOrDie.Components;

namespace DeliverOrDie.Systems;
/// <summary>
/// Handles entities with <see cref="TimeToLive"/> component. They will be destroyed when their
/// <see cref="TimeToLive.Time"/> reaches zero.
/// </summary>
internal class TimeToLiveSystem : GameSystem<TimeToLive>
{
    public TimeToLiveSystem(GameState gameState)
        : base(gameState) { }

    protected override void Update(ref TimeToLive timeToLive)
    {
        timeToLive.Time -= GameState.Elapsed * GameState.Game.Speed;

        if (timeToLive.Time <= 0.0f)
            GameState.DestroyEntity(timeToLive.EntityIndex);
    }
}
