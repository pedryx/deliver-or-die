using DeliverOrDie.Components;
using DeliverOrDie.Events;
using DeliverOrDie.Extensions;
using DeliverOrDie.Resources;
using HypEcs;

using Microsoft.Xna.Framework;

using System;

namespace DeliverOrDie.Systems;
/// <summary>
/// Handles behavior of zombies.
/// </summary>
internal class ZombieSystem : GameSystem<Transform, Movement, Animation, ZombieBehavior>
{
    private const float moveDistance = 1000.0f;
    private const float attackDistance = 81.0f;
    private const float zombieSoundMin = 2.0f;
    private const float zombieSoundMax = 10.0f;

    private readonly Entity player;
    private readonly World ecsWorld;

    private Vector2 playerPosition;

    public ZombieSystem(GameState gameState, Entity player)
        : base(gameState)
    {
        this.player = player;
        ecsWorld = gameState.ECSWorld;
    }

    protected override void PreUpdate()
    {
        playerPosition = ecsWorld.GetComponent<Transform>(player).Position;
    }

    protected override void Update
    (
        ref Transform transform,
        ref Movement movement,
        ref Animation animation,
        ref ZombieBehavior zombie
    )
    {
        var components = new ZombieComponents()
        {
            Transform = ref transform,
            Movement = ref movement,
            Animation = ref animation,
            Zombie = ref zombie,
        };

        switch (zombie.State)
        {
            case ZombieBehavior.BehaviorState.Idle:
                HandleIdle(components);
                break;
            case ZombieBehavior.BehaviorState.Move:
                HandleMove(components);
                break;
            case ZombieBehavior.BehaviorState.Attack:
                HandleAttack(components);
                break;
        }
    }

    private void HandleIdle(ZombieComponents components)
    {
        if (NearPlayer(components.Transform.Position, moveDistance))
        {
            components.Animation.Frames = Animations.Zombie.Move;
            components.Zombie.State = ZombieBehavior.BehaviorState.Move;

            MoveTowardsPlayer(components);
        }
    }

    private void HandleMove(ZombieComponents components)
    {
        SoundSequences.Zombie.Walk.Play(0.07f);

        components.Zombie.ElapsedZombieSound -= GameState.Elapsed;
        if (components.Zombie.ElapsedZombieSound <= 0.0f)
        {
            GameState.Game.SoundManager["Zombie Sound"].Play(0.1f);
            components.Zombie.ElapsedZombieSound += GameState.Game.Random.NextSingle(zombieSoundMin, zombieSoundMax);
        }

        if (!NearPlayer(components.Transform.Position, moveDistance))
        {
            components.Movement.Speed = 0.0f;
            components.Animation.Frames = Animations.Zombie.Idle;
            components.Zombie.State = ZombieBehavior.BehaviorState.Idle;
        }
        else if (NearPlayer(components.Transform.Position, attackDistance))
        {
            components.Animation.FrameIndex = 0;
            components.Animation.Frames = Animations.Zombie.Attack;
            components.Zombie.State = ZombieBehavior.BehaviorState.Attack;

            components.Movement.Speed = 0.0f;
            DealDamageToPlayer(components);
        }
        else
        {
            MoveTowardsPlayer(components);
        }
    }

    private void HandleAttack(ZombieComponents components)
    {
        components.Zombie.Elapsed += GameState.Elapsed * GameState.Game.Speed;
        if (components.Zombie.Elapsed >= components.Zombie.AttackDuration)
        {
            components.Zombie.Elapsed = 0;
            MoveTowardsPlayer(components);

            if (NearPlayer(components.Transform.Position, attackDistance))
            {
                components.Movement.Speed = 0.0f;
                DealDamageToPlayer(components);
            }
            else if (NearPlayer(components.Transform.Position, moveDistance))
            {
                components.Animation.Frames = Animations.Zombie.Move;
                components.Zombie.State = ZombieBehavior.BehaviorState.Move;
            }
            else
            {
                components.Movement.Speed = 0.0f;
                components.Animation.Frames = Animations.Zombie.Idle;
                components.Zombie.State = ZombieBehavior.BehaviorState.Idle;
            }
        }
    }

    private void DealDamageToPlayer(ZombieComponents components)
    {
        GameState.Game.SoundManager["Zombie Attack Sound"].Play(0.1f);
        ref Health health = ref ecsWorld.GetComponent<Health>(player);

        health.Current -= components.Zombie.Damage;
        if (health.Current <= 0)
        {
            health.OnDead?.Invoke(this, new CollisionEventArgs
            (
                GameState.GetEntity(ecsWorld.GetComponent<Player>(player).EntityIndex),
                GameState.GetEntity(components.Zombie.EntityIndex)
            ));
            GameState.DestroyEntity(ecsWorld.GetComponent<Player>(player).EntityIndex);
        }
    }

    /// <summary>
    /// Start movement of zombie towards player.
    /// </summary>
    private void MoveTowardsPlayer(ZombieComponents components)
    {
        Vector2 directionVector = playerPosition - components.Transform.Position;
        float direction = MathF.Atan2(directionVector.Y, directionVector.X);

        components.Transform.Rotation = direction;
        components.Movement.Direction = direction;
        components.Movement.Speed = components.Zombie.MoveSpeed;
    }

    /// <summary>
    /// Determine if zombie is close enough to player.
    /// </summary>
    /// <param name="position">Zombie position.</param>
    /// <param name="distance">How close should zombie be.</param>
    /// <returns>True if zombie is close enough to player.</returns>
    private bool NearPlayer(Vector2 position, float distance)
        => Vector2.Distance(playerPosition, position) < distance;

    private ref struct ZombieComponents
    {
        public ref Transform Transform;
        public ref Movement Movement;
        public ref Animation Animation;
        public ref ZombieBehavior Zombie;
    }
}
