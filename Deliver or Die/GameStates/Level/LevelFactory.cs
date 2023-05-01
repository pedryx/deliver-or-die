using DeliverOrDie.Components;
using DeliverOrDie.Extensions;
using DeliverOrDie.Resources;

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
    private const int bloodSpatCount = 5;
    private const float bloodSpatOffset = 80.0f;

    private readonly World ecsWorld;
    private readonly TextureManager textureManager;
    private readonly SoundManager soundManager;
    private readonly LevelState levelState;
    private readonly Random random;

    private int entityIndex;

    public LevelFactory(LevelState levelState)
    {
        ecsWorld = levelState.ECSWorld;
        textureManager = levelState.Game.TextureManager;
        soundManager = levelState.Game.SoundManager;
        random = levelState.Game.Random;
        this.levelState = levelState;
    }

    private EntityBuilder CreateEntity()
    {
        entityIndex = levelState.GetNextIndex();
        return ecsWorld.Spawn();
    }

    public Entity CreateGrassTile(Texture2D texture, Vector2 position)
    {
        Entity grassTile = CreateEntity()
            .Add(new Transform(position))
            .Add(new Appearance(texture)
            {
                Origin = Vector2.Zero,
                LayerDepth = 1.0f,
            })
            .Add<Background>()
            .Id();
        levelState.AddEntity(grassTile);

        return grassTile;
    }

    public Entity CreateBloodSplat(Vector2 position, float direction, bool dead = false)
    {
        int index = levelState.Game.Random.Next(bloodSpatCount);

        Entity bloodSplat = CreateEntity()
            .Add(new Transform(position)
            {
                Rotation = direction,
            })
            .Add(new Appearance(textureManager[$"bloodsplats_000{(dead ? 7 : (index + 1))}"])
            {
                RotationOffset = dead
                    ? (-MathHelper.Pi / 2.0f)
                    : ((index == 4 ? -MathF.PI / 4.0f: (3 * MathF.PI / 4.0f)) - (MathF.PI / 2.0f)),
                LayerDepth = 0.2f,
                Color = new Color(0.5f, 0.0f, 0.0f, 0.4f),
                ScaleOffset = dead ? 1.0f : 0.7f,
            })
            .Add<Background>()
            .Id();
        levelState.AddEntity(bloodSplat);

        return bloodSplat;
    }

    public Entity CreateBullet(Vector2 position, float direction, float damage)
    {
        Entity bullet = CreateEntity()
            .Add(new Transform(position)
            {
                Rotation = direction,
            })
            .Add(new Appearance(textureManager["bullet"], 0.007f)
            {
                Color = Color.Gold,
                RotationOffset = -MathF.PI / 2.0f,
            })
            .Add(new Movement(3000.0f, direction))
            .Add(new Collider(entityIndex, 1.0f, Collider.Layers.Bullet, damage)
            {
                DamageLayer = Collider.Layers.Zombie,
                CollisionLayer = Collider.Layers.Zombie,
                DestroyOnImpact = true,
            })
            .Add(new TimeToLive(entityIndex, 5.0f))
            .Id();
        levelState.AddEntity(bullet);

        return bullet;
    }

    public Entity CreatePlayer()
    { 
        Entity player = CreateEntity()
            .Add(new Transform())
            .Add(new Appearance(textureManager["survivor-idle_rifle_0"], 0.5f)
            {
                Origin = new Vector2(90.0f, 120.0f),
            })
            .Add(new Player(entityIndex, 5)
            {
                MoveSpeed = 500.0f,
                ReloadTime = 2.0f,
                ShootingSpeed = 5.0f,
                Damage = 1.0f,
            })
            .Add(new Animation(Animations.Player.Idle, 0.06f))
            .Add(new Collider(entityIndex, 30.0f, Collider.Layers.Player)
            {
                CollisionLayer = Collider.Layers.Zombie | Collider.Layers.Obstacle,
                ReactionLayer = Collider.Layers.DeliverySpot,
                OnCollision = (sender, e) =>
                {
                    if (ecsWorld.GetComponent<DeliverySpot>(e.Target).Index == levelState.QuestDeliverySpotIndex)
                        levelState.CompleteDelivery();
                },
            })
            .Add(new Movement())
            .Add(new Health(4.0f)
            {
                OnDead = (sender, e) =>
                {
                    levelState.Camera.Target = null;
                    levelState.GameOver();
                }
            })
            .Id();
        levelState.AddEntity(player);

        return player;
    }

    public Entity CreateZombie(Vector2 position, float speed, float damage, float health)
    {
        float animationTimePerFrame = 0.125f * (speed / 100.0f);

        Entity zombie = CreateEntity()
            .Add(new Transform(position)
            {
                Rotation = random.NextAngle(),
            })
            .Add(new Appearance(textureManager["skeleton-idle_0"], 0.44f))
            .Add(new Animation(Animations.Zombie.Idle, animationTimePerFrame))
            .Add<Movement>()
            .Add(new ZombieBehavior(entityIndex)
            {
                MoveSpeed = 100.0f,
                AttackDuration = animationTimePerFrame * Animations.Zombie.Attack.Count,
                Damage = damage,
            })
            .Add(new Collider(entityIndex, 50.0f, Collider.Layers.Zombie)
            {
                CollisionLayer = Collider.Layers.Zombie,
            })
            .Add(new Health(health)
            {
                OnDead = (sender, e) =>
                {
                    levelState.Game.GameStatistics.Increment(Statistics.ZombiesKilled, 1.0f);

                    Vector2 playerPosition = ecsWorld.GetComponent<Transform>(levelState.Player).Position;
                    Vector2 currentPosition = ecsWorld.GetComponent<Transform>(e.Self).Position;
                    float direction = MathUtils.VectorToAngle(currentPosition - playerPosition);

                    CreateBloodSplat(currentPosition, direction, true);
                    soundManager["bigmonster_die"].Play(0.1f);
                },
                OnHit = (sender, e) =>
                {
                    Vector2 playerPosition = ecsWorld.GetComponent<Transform>(levelState.Player).Position;
                    Vector2 currentPosition = ecsWorld.GetComponent<Transform>(e.Self).Position;
                    Vector2 directionVector = currentPosition - playerPosition;
                    directionVector.Normalize();
                    float direction = MathUtils.VectorToAngle(directionVector);

                    CreateBloodSplat(currentPosition + directionVector * bloodSpatOffset, direction);
                    soundManager["qubodupImpactMeat02"].Play(0.1f);
                }
            })
            .Id();
        levelState.AddEntity(zombie);

        return zombie;
    }

    public Entity CreateDeliverySpot(Vector2 position, int deliverySpotIndex)
    { 
        Entity deliverySpot = CreateEntity()
            .Add(new Transform(position))
            .Add(new Appearance(textureManager["circle"], 0.5f)
            {
                Color = Color.DarkGray,
                LayerDepth = 0.1f,
            })
            .Add(new Collider(entityIndex, 128.0f, Collider.Layers.DeliverySpot))
            .Add(new DeliverySpot(deliverySpotIndex))
            .Add<Background>()
            .Id();
        levelState.AddEntity(deliverySpot);

        return deliverySpot;
    }
}
