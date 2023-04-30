using DeliverOrDie.Components;

using HypEcs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeliverOrDie.UI.Elements;
internal class DirectionMarker : UIElement
{
    private const float offset = 120.0f;

    private Image marker;

    public Entity Destination;
    public Entity TrackedEntity;

    protected override void Initialize()
    {
        Texture2D texture = Owner.GameState.Game.TextureManager["arrow-right"];
        marker = new Image()
        {
            Texture = texture,
            SourceRectangle = new Rectangle(0, 0, 64, 64),
            Origin = new Vector2(32.0f, 32.0f),
        };

        AddChild(marker);
    }

    public override void Update(float elapsed, Vector2 position)
    {
        Vector2 entityPosition = Owner.GameState.ECSWorld.GetComponent<Transform>(TrackedEntity).Position;
        Vector2 destinationPosition = Owner.GameState.ECSWorld.GetComponent<Transform>(Destination).Position;

        Vector2 directionVector = Vector2.Normalize(destinationPosition - entityPosition);
        float direction = MathUtils.VectorToAngle(directionVector);
        marker.Rotation = direction;
        marker.Offset = directionVector * offset + Owner.GameState.Game.Resolution / 2.0f;

        base.Update(elapsed, position);
    }
}
