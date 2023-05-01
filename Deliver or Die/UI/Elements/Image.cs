using DeliverOrDie.Extensions;

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

    public override Vector2 Size => (SourceRectangle == null
        ? Texture == null ? Vector2.Zero : new Vector2(Texture.Width, Texture.Height)
        : SourceRectangle.Value.Size.ToVector2()) * Scale;

    public Image(float scale = 1.0f)
    {
        Scale = scale;
    }

    public Image(Texture2D texture, float scale = 1.0f)
        : this(scale)
    {
        Texture = texture;
        Origin = texture.GetSize() / 2.0f;
    }

    public override void Draw(float elapsed, Vector2 position)
    {
        if (Texture == null)
            return;

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
