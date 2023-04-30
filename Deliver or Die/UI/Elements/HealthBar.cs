using DeliverOrDie.Components;

using HypEcs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie.UI.Elements;
internal class HealthBar : UIElement
{
    private Texture2D healthBarTexture;
    private Image reamingHealthBar;

    public Entity TrackedEntity;

    protected override void Initialize()
    {
        healthBarTexture = Owner.GameState.Game.TextureManager["Healthbar"];

        reamingHealthBar = CreateHealthBarImage(Color.Red);

        AddChild(CreateHealthBarImage(Color.White));
        AddChild(reamingHealthBar);
    }

    private Image CreateHealthBarImage(Color color)
        => new()
        {
            Texture = healthBarTexture,
            Origin = new Vector2(healthBarTexture.Width / 2.0f, healthBarTexture.Height),
            Color = color,
            Scale = 1.4f,
        };

    public override void Update(float elapsed, Vector2 position)
    {
        Health health = Owner.GameState.ECSWorld.GetComponent<Health>(TrackedEntity);

        reamingHealthBar.SourceRectangle = new Rectangle()
        {
            Height = healthBarTexture.Height,
            Width = (int)(healthBarTexture.Width * (health.Current / health.Max)),
        };

        base.Update(elapsed, position);
    }
}
