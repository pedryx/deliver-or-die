using DeliverOrDie.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

namespace DeliverOrDie.Systems;
/// <summary>
/// Handles control of player character.
/// </summary>
internal class PlayerControlSystem : GameSystem<Transform, PlayerControl>
{
    private const Keys upKey    = Keys.W;
    private const Keys leftKey  = Keys.A;
    private const Keys downKey  = Keys.S;
    private const Keys rightKey = Keys.D;

    public PlayerControlSystem(GameState gameState)
        : base(gameState) { }

    protected override void Update(ref Transform transform, ref PlayerControl playerControl)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        MouseState mouseState = Mouse.GetState();

        Vector2 movementdirection = new();
        if (keyboardState.IsKeyDown(upKey))
            movementdirection += -Vector2.UnitY;
        if (keyboardState.IsKeyDown(leftKey))
            movementdirection += -Vector2.UnitX;
        if (keyboardState.IsKeyDown(downKey))
            movementdirection +=  Vector2.UnitY;
        if (keyboardState.IsKeyDown(rightKey))
            movementdirection +=  Vector2.UnitX;

        if (movementdirection != Vector2.Zero)
            movementdirection.Normalize();

        transform.Position += movementdirection * playerControl.Speed * GameState.Elapsed * GameState.Game.Speed;

        Vector2 lookDirection = mouseState.Position.ToVector2() - GameState.Game.Resolution / 2;
        transform.Rotation = MathF.Atan2(lookDirection.Y, lookDirection.X);
        //transform.Rotation = MathUtils.NormalizeAngle(transform.Rotation);
    }
}
