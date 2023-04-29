using HypEcs;

using System.Collections.Generic;

namespace DeliverOrDie.Systems;
/// <summary>
/// Destroy added entities after their time to live expires.
/// </summary>
internal class TimeToLiveSystem : GameSystem
{
    private readonly List<TimeToLivePair> entities = new();

    public TimeToLiveSystem(GameState gameState)
        : base(gameState) { }

    protected override void Update()
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            entities[i].ReamingTime -= GameState.Elapsed * GameState.Game.Speed;
            if (entities[i].ReamingTime <= 0.0f)
            {
                GameState.ECSWorld.Despawn(entities[i].Entity);
                entities.RemoveAt(i);
            }
        }
    }

    public void Add(Entity entity, float time)
    {
        entities.Add(new TimeToLivePair()
        {
            Entity = entity,
            ReamingTime = time,
        });
    }

    private class TimeToLivePair
    {
        public Entity Entity;
        public float ReamingTime;
    }
}
