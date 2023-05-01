using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

namespace DeliverOrDie.UI.Elements;
internal class Button : UIElement
{
    private MouseState lastMouseState = Mouse.GetState();
    private bool lastHover = false;
    private Color color;

    public Image Image { get; private set; } = new();
    public Label Label { get; private set; } = new();

    public event EventHandler OnClick;

    public Color? HoverColor = null;

    protected override void Initialize()
    {
        AddChild(Image);
        AddChild(Label);
    }

    public override void Update(float elapsed, Vector2 position)
    {
        MouseState mouseState = Mouse.GetState();

        var imageRectangle = new Rectangle()
        {
            Location = (position - Image.Origin * Image.Scale).ToPoint(),
            Size = Image.Size.ToPoint(),
        };
        var labelRectangle = new Rectangle()
        {
            Location = (position - Label.Origin * Label.Scale).ToPoint(),
            Size = Label.Size.ToPoint(),
        };

        if (imageRectangle.Contains(mouseState.Position) || labelRectangle.Contains(mouseState.Position))
        {
            if (!lastHover)
            {
                if (HoverColor != null)
                {
                    color = Label.Color;
                    Label.Color = HoverColor.Value;
                }
            }

            if (mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
                OnClick?.Invoke(this, new EventArgs());

            lastHover = true;
        }
        else
        {
            if (lastHover)
            {
                if (HoverColor != null)
                {
                    Label.Color = color;
                }
            }

            lastHover = false;
        }

        lastMouseState = mouseState;

        base.Update(elapsed, position);
    }
}
