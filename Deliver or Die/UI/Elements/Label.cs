using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie.UI.Elements;
/// <summary>
/// Represent simple label UI element.
/// </summary>
internal class Label : UIElement
{
    public SpriteFont Font;
    public string Text = "";
    public Color Color = Color.Black;
    public float Rotation;
    public Vector2 Origin;
    public float Scale = 1.0f;
    public SpriteEffects SpriteEffects;
    public float LayerDepth;

    public override void Draw(float elapsed, Vector2 position)
    {
        Owner.GameState.Game.SpriteBatch.DrawString
        (
            Font,
            Text,
            position,
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
