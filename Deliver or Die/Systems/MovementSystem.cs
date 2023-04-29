using DeliverOrDie.Components;

using System;

namespace DeliverOrDie.Systems;
/// <summary>
/// Handles movement of entities.
/// </summary>
internal class MovementSystem : GameSystem<Transform, Movement>
{
    public MovementSystem(GameState gameState)
        : base(gameState) { }

    protected override void Update(ref Transform transform, ref Movement movement)
    {
        float distance = movement.Speed * GameState.Elapsed * GameState.Game.Speed;

        transform.Position.X += MathF.Cos(movement.Direction) * distance;
        transform.Position.Y += MathF.Sin(movement.Direction) * distance;
    }
}
