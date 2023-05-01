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

    public override Vector2 Size => Font == null ? Vector2.Zero : Font.MeasureString(Text) * Scale;

    public Label(float scale = 1.0f)
    {
        Scale = scale;
    }

    public Label(SpriteFont font, string text = "", float scale = 1.0f)
        : this(scale)
    {
        Font = font;
        Text = text;
        Origin = font.MeasureString(Text) / 2.0f;
    }

    public override void Draw(float elapsed, Vector2 position)
    {
        if (Font == null)
            return;

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
