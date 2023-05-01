using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace DeliverOrDie.UI.Elements;
internal class Timer : UIElement
{
    private Label label;

    public float Time;

    public event EventHandler OnFinish;

    protected override void Initialize()
    {
        SpriteFont font = Owner.GameState.Game.FontManager["Comic Sans;64"];
        label = new Label()
        {
            Font = font,
            Color = Color.White,
            Origin = new Vector2(font.MeasureString("00:00").X / 2.0f, 0.0f),
        };
        AddChild(label);
    }

    public override void Update(float elapsed, Vector2 position)
    {
        Time -= elapsed;
        if (Time <= 0.0f)
        {
            Time = 0.0f;
            OnFinish?.Invoke(this, new EventArgs());
        }

        label.Text = $"{(int)Time / 60,2}:{((int)Time % 60).ToString().PadLeft(2, '0')}";

        base.Update(elapsed, position);
    }
}
