using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie.UI.Elements;
/// <summary>
/// Represent simple image UI element.
/// </summary>
internal class Image : UIElement
{
    public Texture2D Texture;
    public Rectangle? SourceRectangle;
    public Color Color = Color.White;
    public float Rotation;
    public Vector2 Origin;
    public float Scale = 1.0f;
    public SpriteEffects SpriteEffects;
    public float LayerDepth;

    public override void Draw(float elapsed, Vector2 position)
    {
        Owner.GameState.Game.SpriteBatch.Draw
        (
            Texture,
            position,
            SourceRectangle,
            Color,
            Rotation,
            Origin,
            Scale,
            SpriteEffects,
            LayerDepth
        );

        base.Draw(elapsed, position);
    }
}
