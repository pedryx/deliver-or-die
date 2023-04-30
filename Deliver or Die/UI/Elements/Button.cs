using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

namespace DeliverOrDie.UI.Elements;
internal class Button : UIElement
{
    private MouseState lastMouseState = Mouse.GetState();

    public Image Image { get; private set; } = new();

    public override Vector2 Size => Image.Size;

    public event EventHandler OnClick;

    protected override void Initialize()
    {
        AddChild(Image);
    }

    public override void Update(float elapsed, Vector2 position)
    {
        MouseState mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
        {
            var rectangle = new Rectangle()
            {
                Location = (position - Image.Origin * Image.Scale).ToPoint(),
                Size = Size.ToPoint(),
            };

            if (rectangle.Contains(mouseState.Position))
                OnClick?.Invoke(this, new EventArgs());
        }

        lastMouseState = mouseState;

        base.Update(elapsed, position);
    }
}
