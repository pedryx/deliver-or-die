using DeliverOrDie.Components;

using HypEcs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie.Systems;
/// <summary>
/// Draws appearances.
/// </summary>
internal class RenderSystem : GameSystem
{
    private readonly Camera camera;
    private readonly SpriteBatch spriteBatch;
    private readonly Query<Transform, Appearance, Background> firstBatch;
    private readonly Query<Transform, Appearance> secondBatch;
    private readonly Query<Transform, Appearance, Foreground> thirdBatch;

    public RenderSystem(GameState gameState) 
        : base(gameState)
    {
        camera = gameState.Camera;
        spriteBatch = gameState.Game.SpriteBatch;

        firstBatch = GameState.ECSWorld.Query<Transform, Appearance, Background>().Build();
        secondBatch = GameState.ECSWorld.Query<Transform, Appearance>().Not<Background>().Not<Foreground>().Build();
        thirdBatch = GameState.ECSWorld.Query<Transform, Appearance, Foreground>().Build();
    }

    protected override void Update()
    {
        // render first batch
        spriteBatch.Begin(transformMatrix: camera.GetTransformMatrix());
        firstBatch.Run((count, transforms, appearances, backgrounds) =>
        {
            for (int i = 0; i < count; i++)
            {
                Draw(ref transforms[i], ref appearances[i]);
            }
        });
        spriteBatch.End();

        // render second batch
        spriteBatch.Begin(transformMatrix: camera.GetTransformMatrix());
        secondBatch.Run((count, transforms, appearances) =>
        {
            for (int i = 0; i < count; i++)
            {
                Draw(ref transforms[i], ref appearances[i]);
            }
        });
        spriteBatch.End();

        // render third batch
        spriteBatch.Begin();
        thirdBatch.Run((count, transforms, appearances, foregrounds) =>
        {
            for (int i = 0; i < count; i++)
            {
                Draw(ref transforms[i], ref appearances[i], false);
            }
        });
        spriteBatch.End();
    }

    protected void Draw(ref Transform transform, ref Appearance appearance, bool clipping = true)
    {
        if (clipping)
        {
            float distance = Vector2.Distance(camera.Position, transform.Position + appearance.PositionOffset);
            if (distance > (GameState.Game.Resolution.X / 2.0f) * (1.0f / camera.Scale) * 2.5f)
                return;
        }

        spriteBatch.Draw
        (
            appearance.Texture,
            transform.Position + appearance.PositionOffset,
            appearance.SourceRectangle,
            appearance.Color,
            transform.Rotation + appearance.RotationOffset,
            appearance.Origin,
            transform.Scale * appearance.ScaleOffset,
            appearance.SpriteEffects,
            appearance.LayerDepth
        );
    }
}
