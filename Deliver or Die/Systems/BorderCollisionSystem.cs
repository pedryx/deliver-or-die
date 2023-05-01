using DeliverOrDie.Components;

using Microsoft.Xna.Framework;

using System;

namespace DeliverOrDie.Systems;
internal class BorderCollisionSystem : GameSystem<Transform, Collider, Movement>
{
    public readonly Vector2 borderSize;

    public BorderCollisionSystem(GameState gameState, Vector2 borderSize)
        : base(gameState)
    {
        this.borderSize = borderSize;
    }

    protected override void Update(ref Transform transform, ref Collider collider, ref Movement movement)
    {
        Vector2 halfBorder = borderSize / 2.0f;

        float top    = MathF.Abs(-halfBorder.Y - (transform.Position.Y - collider.Radius)) - collider.Radius;
        float left   = MathF.Abs(-halfBorder.X - (transform.Position.X - collider.Radius)) - collider.Radius;
        float down   = MathF.Abs(+halfBorder.Y - (transform.Position.Y + collider.Radius)) - collider.Radius;
        float right = MathF.Abs(+halfBorder.X - (transform.Position.X + collider.Radius))- collider.Radius;

        if (top < 0.0f)
            transform.Position.Y -= top;
        if (left < 0.0f)
            transform.Position.X -= left;
        if (down < 0.0f)
            transform.Position.Y += down;
        if (right < 0.0f)
            transform.Position.X += right;
    }
}
