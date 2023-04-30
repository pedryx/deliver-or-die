using DeliverOrDie.Components;

using HypEcs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace DeliverOrDie.UI.Elements;
internal class AmmoCounter : UIElement
{
    private const int ammoTextPadding = 3;
    
    private readonly Vector2 padding = new(35.0f, 28.0f);

    private Label ammoCountLabel;

    /// <summary>
    /// Tracked entity with <see cref="Player"/> component.
    /// </summary>
    public Entity TrackedEntity;

    protected override void Initialize()
    {
        Texture2D texture = Owner.GameState.Game.TextureManager["bullet"];
        var image = new Image()
        {
            Texture = texture,
            Rotation = MathF.PI / 4.0f,
            Origin = new Vector2(0.0f, texture.Height),
            Scale = 0.05f,
        };

        SpriteFont font = Owner.GameState.Game.FontManager[$"Comic Sans;{(int)(image.Size.Y * 0.7f)}"];
        ammoCountLabel = new Label()
        {
            Font = font,
            Color = Color.White,
            Offset = new Vector2(image.Size.X, 0.0f) + padding,
            Origin = new Vector2(0.0f, (int)image.Size.Y),
        };
        SetText();
        
        AddChild(ammoCountLabel);
        AddChild(image);
    }

    public override void Update(float elapsed, Vector2 position)
    {
        SetText();

        base.Update(elapsed, position);
    }

    public override void Draw(float elapsed, Vector2 position)
    {
        base.Draw(elapsed, position);
    }

    private void SetText()
    {
        ref Player player = ref Owner.GameState.ECSWorld.GetComponent<Player>(TrackedEntity);
        ammoCountLabel.Text = $"{player.Ammo,ammoTextPadding}/{player.MaxAmmo}";
    }
}
