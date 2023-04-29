using DeliverOrDie.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

namespace DeliverOrDie.Systems;
/// <summary>
/// Handles control of player character.
/// </summary>
internal class PlayerControlSystem : GameSystem<Transform, Animation, PlayerControl>
{
    private const Keys upKey    = Keys.W;
    private const Keys leftKey  = Keys.A;
    private const Keys downKey  = Keys.S;
    private const Keys rightKey = Keys.D;
    private const Keys reloadKey = Keys.R;

    /// <summary>
    /// Determine if movement occur during last frame.
    /// </summary>
    private bool lastMovement;
    private MouseState lastMouseState;
    private KeyboardState lastKeyboardState;

    public PlayerControlSystem(GameState gameState)
        : base(gameState) { }

    protected override void Update(ref Transform transform, ref Animation animation, ref PlayerControl playerControl)
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
        bool currentMovement = movementdirection != Vector2.Zero;

        if (currentMovement)
            movementdirection.Normalize();

        transform.Position += movementdirection * playerControl.MoveSpeed * GameState.Elapsed * GameState.Game.Speed;

        if (currentMovement && !lastMovement)
            animation.Frames = Animations.Player.Move;
        if (!currentMovement && lastMovement)
            animation.Frames = Animations.Player.Idle;

        Vector2 lookDirection = mouseState.Position.ToVector2() - GameState.Game.Resolution / 2;
        transform.Rotation = MathF.Atan2(lookDirection.Y, lookDirection.X);

        if (playerControl.Shooting)
        {
            playerControl.ShootingElapsed += GameState.Elapsed * GameState.Game.Speed;
            if (playerControl.ShootingElapsed >= 1.0f / playerControl.ShootingSpeed)
            {
                playerControl.ShootingElapsed = 0.0f;
                playerControl.Shooting = false;
                if (!playerControl.Reloading)
                    animation.Frames = Animations.Player.Idle;
            }
        }

        if (playerControl.Reloading)
        {
            playerControl.ReloadingElapsed += GameState.Elapsed * GameState.Game.Speed;
            if (playerControl.ReloadingElapsed >= playerControl.ReloadTime)
            {
                playerControl.ReloadingElapsed = 0.0f;
                playerControl.Reloading = false;
                animation.TimePerFrame = playerControl.AnimationTimePerFrame;
                animation.Frames = Animations.Player.Idle;
                // TODO: actually reload ammo
            }
        }
        else
        {
            if (lastKeyboardState.IsKeyUp(reloadKey) && keyboardState.IsKeyDown(reloadKey))
            {
                playerControl.AnimationTimePerFrame = animation.TimePerFrame;
                animation.Frames = Animations.Player.Reload;
                animation.FrameIndex = 0;
                animation.TimePerFrame = playerControl.ReloadTime / animation.Frames.Count;
                playerControl.Reloading = true;
            }
            else if (mouseState.LeftButton == ButtonState.Pressed && !playerControl.Shooting)
            {
                animation.Frames = Animations.Player.Shoot;
                if (animation.FrameIndex >= animation.Frames.Count)
                    animation.FrameIndex = 0;
                playerControl.Shooting = true;
                // TODO: actually fire ammo
            }
        }

        lastMovement = currentMovement;
        lastMouseState = mouseState;
        lastKeyboardState = keyboardState;
    }
}
