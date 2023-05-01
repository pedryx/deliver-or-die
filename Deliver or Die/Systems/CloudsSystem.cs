using DeliverOrDie.Components;
using DeliverOrDie.Extensions;
using DeliverOrDie.GameStates.Level;

using Microsoft.Xna.Framework;

using System;

namespace DeliverOrDie.Systems;
internal class CloudsSystem : GameSystem<Transform, Cloud>
{
    private const int cloudCount = 50_000;

    private readonly LevelFactory factory;
    private readonly Random random;
    private readonly Vector2 worldSize;

    public CloudsSystem(GameState gameState, LevelFactory factory, Vector2 worldSize)
        : base(gameState)
    {
        this.factory = factory;
        this.worldSize = worldSize;

        random = gameState.Game.Random;

        for (int i = 0; i < cloudCount; i++)
            factory.CreateCloud(random.Nextvector2(worldSize) - worldSize / 2.0f);
    }

    protected override void Update(ref Transform transform, ref Cloud cloud)
    {
        if (transform.Position.X < (-worldSize.X / 2.0f - 500.0f))
        {
            GameState.DestroyEntity(cloud.EntityIndex);

            factory.CreateCloud(new Vector2
            (
                worldSize.X / 2.0f + 500.0f,
                random.NextSingle(-worldSize.Y / 2.0f, worldSize.Y / 2.0f))
            );
        }
    }
}
