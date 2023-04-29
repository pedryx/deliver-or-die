using DeliverOrDie.Components;

using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie.Systems;
/// <summary>
/// Draws appearances.
/// </summary>
internal class RenderSystem : GameSystem<Transform, Appearance>
{
    private readonly Camera camera;
    private readonly SpriteBatch spriteBatch;

    public RenderSystem(GameState gameState) 
        : base(gameState)
    {
        camera = gameState.Camera;
        spriteBatch = gameState.Game.SpriteBatch;
    }

    protected override void PreUpdate()
    {
        spriteBatch.Begin(transformMatrix: camera.GetTransformMatrix());
    }

    protected override void Update(ref Transform transform, ref Appearance appearance)
    {
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

    protected override void PostUpdate()
    {
        spriteBatch.End();
    }
}
