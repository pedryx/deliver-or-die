using DeliverOrDie.Components;
using DeliverOrDie.GameStates.Level;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

using System;
using System.IO;
using System.Linq;

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
    private const Keys debugKey = Keys.F3;

    private const float bulletDirectionOffset = 0.4f;
    private const float bulletPositionOffset = 40.0f;

    private readonly LevelFactory factory;

    /// <summary>
    /// Determine if movement occur during last frame.
    /// </summary>
    private bool lastMovement;
    private MouseState lastMouseState;
    private KeyboardState lastKeyboardState;

    public PlayerControlSystem(GameState gameState, LevelFactory factory)
        : base(gameState) 
    {
        this.factory = factory;
    }

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
        {
            movementdirection.Normalize();
            SoundSequences.Player.Walk.Play(0.3f);
        }

        transform.Position += movementdirection * playerControl.MoveSpeed * GameState.Elapsed * GameState.Game.Speed;

        if (currentMovement && !lastMovement)
            animation.Frames = Animations.Player.Move;
        if (!currentMovement && lastMovement)
            animation.Frames = Animations.Player.Idle;

        Vector2 lookDirection = mouseState.Position.ToVector2() - GameState.Game.Resolution / 2;
        lookDirection.Normalize();
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

                GameState.Game.SoundManager["assaultriflereload1"].Play(0.4f);
            }
            else if (mouseState.LeftButton == ButtonState.Pressed && !playerControl.Shooting)
            {
                animation.Frames = Animations.Player.Shoot;
                if (animation.FrameIndex >= animation.Frames.Count)
                    animation.FrameIndex = 0;
                playerControl.Shooting = true;
                GameState.Game.SoundManager["lmg_fire01"].Play(0.5f);
                
                float spawnDirectionAngle = transform.Rotation + bulletDirectionOffset;
                Vector2 spawnDirection = new Vector2(MathF.Cos(spawnDirectionAngle), MathF.Sin(spawnDirectionAngle));
                Vector2 spawnPosition = transform.Position + spawnDirection * bulletPositionOffset;

                factory.CreateBullet(spawnPosition, transform.Rotation);
                // TODO: descrease ammo
            }
        }

        if (lastKeyboardState.IsKeyUp(debugKey) && keyboardState.IsKeyDown(debugKey))
            Debug();

        lastMovement = currentMovement;
        lastMouseState = mouseState;
        lastKeyboardState = keyboardState;
    }

    /// <summary>
    /// Simple debug action which is invoked when <see cref="debugKey"/> has been pressed.
    /// </summary>
    private void Debug()
    {
        string file = Directory.GetFiles("Content/Sounds", "Footstep_Dirt_00.wav", SearchOption.AllDirectories).First();
        
        /*
        var song = Song.FromUri("song", new Uri(file, UriKind.Relative));
        MediaPlayer.Play(song);
        */

        
        var effect = SoundEffect.FromFile(file);
        effect.Play();
        
    }
}
