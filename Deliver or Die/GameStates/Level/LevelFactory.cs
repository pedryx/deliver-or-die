using DeliverOrDie.Components;
using DeliverOrDie.Systems;

using HypEcs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Threading;

namespace DeliverOrDie.GameStates.Level;
/// <summary>
/// Factory for creating entities for <see cref="LevelState"/>.
/// </summary>
internal class LevelFactory
{
    private readonly World ecsWorld;
    private readonly TextureManager textures;
    private readonly TimeToLiveSystem timeToLiveSystem;

    public LevelFactory(LevelState levelState)
    {
        ecsWorld = levelState.ECSWorld;
        textures = levelState.Game.TextureManager;
        timeToLiveSystem = levelState.TimeToLiveSystem;
    }

    public Entity CreateSquare(Vector2 position, float size, Color color, float layerDepth = 0.0f)
    {
        Entity square = ecsWorld.Spawn()
            .Add(Transform.Create(position))
            .Add(new Appearance()
            {
                Texture = textures.Square,
                Color = color,
                ScaleOffset = size,
                Origin = new Vector2(0.5f),
                LayerDepth = layerDepth
            })
            .Id();

        return square;
    }

    public Entity CreateBullet(Vector2 position, float direction)
    {
        Texture2D buletTexture = textures["bullet"];

        Entity bullet = ecsWorld.Spawn()
            .Add(new Transform()
            {
                Position = position,
                Rotation = direction,
                Scale = 1.0f,
            })
            .Add(new Appearance()
            {
                Texture = buletTexture,
                Color = Color.Gold,
                ScaleOffset = 0.007f,
                Origin = new Vector2(buletTexture.Width, buletTexture.Height) / 2.0f,
                RotationOffset = - MathF.PI / 2.0f,
            })
            .Add(new Movement()
            {
                Speed = 3000.0f,
                Direction = direction,
            })
            .Id();
        timeToLiveSystem.Add(bullet, 5.0f);

        return bullet;
    }

    public Entity CreatePlayer()
    {
        Entity player = ecsWorld.Spawn()
            .Add(Transform.Create())
            .Add(new Appearance()
            {
                Texture = textures["survivor-idle_rifle_0"],
                ScaleOffset = 0.5f,
                Origin = new Vector2(90, 120),
                Color = Color.White,
            })
            .Add(new Player()
            {
                MoveSpeed = 500.0f,
                ReloadTime = 2.0f,
                ShootingSpeed = 5.0f,
                Ammo = 5,
                MaxAmmo = 5,
            })
            .Add(new Animation()
            {
                TimePerFrame = 0.06f,
                Frames = Animations.Player.Idle,
            })
            .Id();

        return player;
    }

    public Entity CreateZombie(Vector2 position)
    {
        Entity zombie = ecsWorld.Spawn()
            .Add(Transform.Create(position))
            .Add(Appearance.Create(textures["skeleton-idle_0"], 0.44f))
            .Add(new Animation()
            {
                TimePerFrame = 0.125f,
                Frames = Animations.Zombie.Idle,
            })
            .Id();

        return zombie;
    }
}
