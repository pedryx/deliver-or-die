using DeliverOrDie.Components;
using DeliverOrDie.Resources;

using HypEcs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie.UI.Elements;
internal class BloodOverlay : UIElement
{
    private Texture2D texture;
    private Sound breating;
    private Sound fastBreathing;

    public bool Visible { get; private set; }

    public Entity Target;

    protected override void Initialize()
    {
        texture = Owner.GameState.Game.TextureManager["BloodOverlay"];
        breating = Owner.GameState.Game.SoundManager["Breath_Scared_00"];
        fastBreathing = Owner.GameState.Game.SoundManager["Breath_Scared_01"];
    }

    public override void Update(float elapsed, Vector2 position)
    {
        Health health = Owner.GameState.ECSWorld.GetComponent<Health>(Target);
        if (health.Current / health.Max <= 0.33f)
        {
            Visible = true;
            fastBreathing.Play(0.2f, true);
        }
        else
        {
            Visible = false;
            breating.Play(0.2f, true);
        }

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
