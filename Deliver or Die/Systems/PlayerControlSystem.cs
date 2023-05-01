using DeliverOrDie.Components;
using DeliverOrDie.GameStates.Level;
using DeliverOrDie.Resources;
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
internal class PlayerControlSystem : GameSystem<Transform, Movement, Animation, Player>
{
    private const Keys upKey     = Keys.W;
    private const Keys leftKey   = Keys.A;
    private const Keys downKey   = Keys.S;
    private const Keys rightKey  = Keys.D;
    private const Keys reloadKey = Keys.R;
    private const Keys debugKey  = Keys.F3;

    private const float bulletDirectionOffset =  0.3f;
    private const float bulletPositionOffset  = 55.0f;

    private readonly LevelFactory factory;

    /// <summary>
    /// Determine if movement occur during last frame.
    /// </summary>
    private bool lastMovement;
    private KeyboardState lastKeyboardState;

    public PlayerControlSystem(GameState gameState, LevelFactory factory)
        : base(gameState) 
    {
        this.factory = factory;
    }

    protected override void Update
    (
        ref Transform transform,
        ref Movement  movement,
        ref Animation animation,
        ref Player    player
    )
    {
        KeyboardState keyboardState = Keyboard.GetState();
        MouseState mouseState = Mouse.GetState();

        Vector2 movementDirection = new();
        if (keyboardState.IsKeyDown(upKey))
            movementDirection += -Vector2.UnitY;
        if (keyboardState.IsKeyDown(leftKey))
            movementDirection += -Vector2.UnitX;
        if (keyboardState.IsKeyDown(downKey))
            movementDirection +=  Vector2.UnitY;
        if (keyboardState.IsKeyDown(rightKey))
            movementDirection +=  Vector2.UnitX;
        bool currentMovement = movementDirection != Vector2.Zero;

        if (currentMovement)
        {
            movement.Direction = MathUtils.VectorToAngle(movementDirection);
            SoundSequences.Player.Walk.Play(0.3f);
        }

        if (currentMovement && !lastMovement)
        {
            movement.Speed = player.MoveSpeed;
            if (!player.Reloading)
                animation.Frames = Animations.Player.Move;
        }
        if (!currentMovement && lastMovement)
        {
            movement.Speed = 0.0f;
            if (!player.Reloading)
                animation.Frames = Animations.Player.Idle;
        }

        Vector2 lookDirection = mouseState.Position.ToVector2() - GameState.Game.Resolution / 2;
        lookDirection.Normalize();
        transform.Rotation = MathF.Atan2(lookDirection.Y, lookDirection.X);

        if (player.Shooting)
        {
            player.ShootingElapsed += GameState.Elapsed * GameState.Game.Speed;
            if (player.ShootingElapsed >= 1.0f / player.ShootingSpeed)
            {
                player.ShootingElapsed = 0.0f;
                player.Shooting = false;
                if (!player.Reloading)
                {
                    if (currentMovement)
                        animation.Frames = Animations.Player.Move;
                    else
                        animation.Frames = Animations.Player.Idle;
                }
            }
        }

        if (player.Reloading)
        {
            player.ReloadingElapsed += GameState.Elapsed * GameState.Game.Speed;
            if (player.ReloadingElapsed >= player.ReloadTime)
            {
                player.ReloadingElapsed = 0.0f;
                player.Reloading = false;
                animation.TimePerFrame = player.AnimationTimePerFrame;
                if (currentMovement)
                    animation.Frames = Animations.Player.Move;
                else
                    animation.Frames = Animations.Player.Idle;
                player.Ammo = player.MaxAmmo;
            }
        }
        else
        {
            if (lastKeyboardState.IsKeyUp(reloadKey) && keyboardState.IsKeyDown(reloadKey))
            {
                player.AnimationTimePerFrame = animation.TimePerFrame;
                animation.Frames = Animations.Player.Reload;
                animation.FrameIndex = 0;
                animation.TimePerFrame = player.ReloadTime / animation.Frames.Count;
                player.Reloading = true;

                GameState.Game.SoundManager["assaultriflereload1"].Play(0.4f);
            }
            else if (mouseState.LeftButton == ButtonState.Pressed && !player.Shooting)
            {
                if (player.Ammo <= 0)
                {
                    GameState.Game.SoundManager["gun_fire2"].Play(0.4f, true, 0.1f);
                }
                else
                {

                    animation.Frames = Animations.Player.Shoot;
                    if (animation.FrameIndex >= animation.Frames.Count)
                        animation.FrameIndex = 0;
                    player.Shooting = true;
                    GameState.Game.SoundManager["lmg_fire01"].Play(0.5f);

                    float spawnDirectionAngle = transform.Rotation + bulletDirectionOffset;
                    Vector2 spawnDirection = new(MathF.Cos(spawnDirectionAngle), MathF.Sin(spawnDirectionAngle));
                    Vector2 spawnPosition = transform.Position + spawnDirection * bulletPositionOffset;

                    factory.CreateBullet(spawnPosition, transform.Rotation, player.Damage);
                    player.Ammo--;
                }
            }
        }

        if (lastKeyboardState.IsKeyUp(debugKey) && keyboardState.IsKeyDown(debugKey))
            Debug();

        lastMovement = currentMovement;
        lastKeyboardState = keyboardState;
    }

    /// <summary>
    /// Simple debug action which is invoked when <see cref="debugKey"/> has been pressed.
    /// </summary>
    private static void Debug()
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
