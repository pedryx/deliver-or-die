using DeliverOrDie.Components;
using DeliverOrDie.Systems;

using HypEcs;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace DeliverOrDie.GameStates.Level;
/// <summary>
/// Factory for creating entities for <see cref="LevelState"/>.
/// </summary>
internal class LevelFactory
{
    private readonly World ecsWorld;
    private readonly TextureManager textures;
    private readonly TimeToLiveSystem timeToLiveSystem;
    private readonly LevelState levelState;

    public LevelFactory(LevelState levelState)
    {
        ecsWorld = levelState.ECSWorld;
        textures = levelState.Game.TextureManager;
        timeToLiveSystem = levelState.TimeToLiveSystem;
        this.levelState = levelState;
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
        Texture2D bulletTexture = textures["bullet"];

        Entity bullet = ecsWorld.Spawn()
            .Add(new Transform()
            {
                Position = position,
                Rotation = direction,
                Scale = 1.0f,
            })
            .Add(new Appearance()
            {
                Texture = bulletTexture,
                Color = Color.Gold,
                ScaleOffset = 0.007f,
                Origin = new Vector2(bulletTexture.Width, bulletTexture.Height) / 2.0f,
                RotationOffset = - MathF.PI / 2.0f,
            })
            .Add(new Movement()
            {
                Speed = 3000.0f,
                Direction = direction,
            })
            .Add(new Collider()
            {
                Radius = 1.0f,
                Damage = 1.0f,
                Layer = Collider.Layers.Bullet,
                DamageLayer = Collider.Layers.Zombie,
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
            .Add(new Collider()
            {
                Layer = Collider.Layers.Player,
                Radius = 30.0f,
                CollisionLayer = Collider.Layers.Zombie | Collider.Layers.Obstacle,
                ReactionLayer = Collider.Layers.DeliverySpot,
                Reaction = (data) =>
                {
                    if ((int)data == levelState.QuestDeliverySpotIndex)
                        levelState.CompleteDelivery();
                },
            })
            .Add<Movement>()
            .Add(new Health()
            {
                Max = 5.0f,
                Current = 5.0f,
                EntityIndex = levelState.GetNextIndex(),
                OnDead = (position) =>
                {
                    levelState.Camera.Target = null;
                    // TODO: player on dead
                },
            })
            .Id();

        levelState.AddEntity(player);
        return player;
    }

    public Entity CreateZombie(Vector2 position)
    {
        const float animationTimePerFrame = 0.125f;

        Entity zombie = ecsWorld.Spawn()
            .Add(new Transform()
            {
                Position = position,
                Rotation = levelState.Game.Random.NextAngle(),
                Scale = 1.0f,
            })
            .Add(Appearance.Create(textures["skeleton-idle_0"], 0.44f))
            .Add(new Animation()
            {
                TimePerFrame = animationTimePerFrame,
                Frames = Animations.Zombie.Idle,
            })
            .Add<Movement>()
            .Add(new ZombieBehavior()
            {
                MoveSpeed = 100.0f,
                AttackDuration = animationTimePerFrame * Animations.Zombie.Attack.Count,
                Damage = 1.0f,
            })
            .Add(new Collider()
            {
                Layer = Collider.Layers.Zombie,
                Radius = 50.0f,
                CollisionLayer = Collider.Layers.Player | Collider.Layers.Obstacle,
            })
            .Add(new Health()
            {
                Max = 1.0f,
                Current = 1.0f,
                EntityIndex = levelState.GetNextIndex(),
                // TODO: zombie on dead
            })
            .Id();

        levelState.AddEntity(zombie);
        return zombie;
    }

    public Entity CreateDeliverySpot(Vector2 position, int id)
    {
        Texture2D texture = textures["circle"];

        Entity mailBox = ecsWorld.Spawn()
            .Add(Transform.Create(position))
            .Add(new Appearance()
            {
                Texture = texture,
                Color = Color.DarkGray,
                ScaleOffset = 0.5f,
                Origin = new Vector2(texture.Width, texture.Height) / 2.0f,
                LayerDepth = 1.0f,
            })
            .Add(new Collider()
            {
                Radius = 32.0f,
                Layer = Collider.Layers.DeliverySpot,
                Data = id,
            })
            .Id();

        return mailBox;
    }
}
