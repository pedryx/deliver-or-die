using DeliverOrDie.Components;
using DeliverOrDie.Extensions;
using DeliverOrDie.GameStates.Level;

using Microsoft.Xna.Framework;

using System;

namespace DeliverOrDie.Systems;
internal class CloudsSystem : GameSystem<Transform, Cloud>
{
    private const float border = 700.0f;

    private const int cloudCount = 400;

    private readonly LevelFactory factory;
    private readonly Random random;

    public CloudsSystem(GameState gameState, LevelFactory factory)
        : base(gameState)
    {
        this.factory = factory;

        random = gameState.Game.Random;

        for (int i = 0; i < cloudCount; i++)
            factory.CreateCloud(random.Nextvector2(gameState.Game.Resolution + new Vector2(border * 2.0f))
                - new Vector2(border));
    }

    protected override void Update(ref Transform transform, ref Cloud cloud)
    {
        if (transform.Position.X < -border)
        {
            GameState.DestroyEntity(cloud.EntityIndex);

            factory.CreateCloud(new Vector2
            (
                GameState.Game.Resolution.X + border,
                random.NextSingle(-border, GameState.Game.Resolution.Y + border)
            ));
        }
    }
}
