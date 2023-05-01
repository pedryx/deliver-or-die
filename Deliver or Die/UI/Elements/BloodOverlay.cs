using DeliverOrDie.Components;

using HypEcs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie.UI.Elements;
internal class BloodOverlay : UIElement
{
    private Texture2D texture;

    public bool Visible { get; private set; }

    public Entity Target;

    protected override void Initialize()
    {
        texture = Owner.GameState.Game.TextureManager["BloodOverlay"];
    }

    public override void Update(float elapsed, Vector2 position)
    {
        Health health = Owner.GameState.ECSWorld.GetComponent<Health>(Target);
        if (health.Current / health.Max <= 0.33f)
            Visible = true;
        else
            Visible = false;

        base.Update(elapsed, position);
    }

    public override void Draw(float elapsed, Vector2 position)
    {
        if (Visible)
        {
            Owner.GameState.Game.SpriteBatch.Draw
            (
                texture,
                new Rectangle(Vector2.Zero.ToPoint(), Owner.GameState.Game.Resolution.ToPoint()),
                new Color(0.5f, 0.0f, 0.0f, 0.5f)
            );
        }

        base.Draw(elapsed, position);
    }
}
