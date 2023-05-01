using DeliverOrDie.Components;
using DeliverOrDie.Events;
using DeliverOrDie.Extensions;

using HypEcs;

using Microsoft.Xna.Framework;

namespace DeliverOrDie.Systems;

/// <summary>
/// Handles collision between entities.
/// </summary>
internal class CollisionSystem : GameSystem<Transform, Collider>
{
    private readonly Query<Transform, Collider> query;
    private readonly World ecsWorld;

    public CollisionSystem(GameState gameState)
        : base(gameState)
    {
        ecsWorld = gameState.ECSWorld;
        query = ecsWorld.Query<Transform, Collider>().Build();
    }

    protected override void Update(ref Transform transformReference, ref Collider colliderReference)
    {
        Transform transform = transformReference;
        Collider collider = colliderReference;

        query.Run((count, transforms, colliders) =>
        {
            for (int i = 0; i < count; i++)
            {
                if (collider.EntityIndex == colliders[i].EntityIndex)
                    continue;

                var components = new CollisionComponents()
                {
                    Transform1 = transform,
                    Collider1 = collider,
                    Transform2 = transforms[i],
                    Collider2 = colliders[i],
                };

                HandleCollisions(components);
            }
        });
    }

    private void HandleCollisions(CollisionComponents components)
    {
        float distance = Vector2.Distance(components.Transform1.Position, components.Transform2.Position);
        if (distance > components.Collider1.Radius + components.Collider2.Radius)
            return;

        Entity entity1 = GameState.GetEntity(components.Collider1.EntityIndex);
        Entity entity2 = GameState.GetEntity(components.Collider2.EntityIndex);

        // reaction
        if ((components.Collider1.ReactionLayer & components.Collider2.Layer) > 0)
            components.Collider1.OnCollision?.Invoke(this, new CollisionEventArgs(entity1, entity2));

        // damage
        if ((components.Collider1.DamageLayer & components.Collider2.Layer) > 0)
        {
            if (ecsWorld.HasComponent<Health>(entity2))
            {
                ref Health health2 = ref ecsWorld.GetComponent<Health>(entity2);

                health2.Current -= components.Collider1.Damage;

                if (health2.Current <= 0.0f)
                {
                    health2.OnDead?.Invoke(this, new CollisionEventArgs(entity2, entity1));
                    GameState.DestroyEntity(components.Collider2.EntityIndex);
                }
                else
                {
                    health2.OnHit?.Invoke(this, new CollisionEventArgs(entity2, entity1));
                }
            }
        }

        // impact
        if ((components.Collider1.CollisionLayer & components.Collider2.Layer) > 0)
        {
            if (ecsWorld.HasComponent<Movement>(entity1))
            {
                ref Movement movement = ref ecsWorld.GetComponent<Movement>(entity1);
                ref Transform transform = ref ecsWorld.GetComponent<Transform>(entity1);

                if (movement.Speed != 0.0f)
                {
                    Vector2 direction = components.Transform1.Position - components.Transform2.Position;

                    if (direction == Vector2.Zero)
                        direction = MathUtils.AngleToVector(GameState.Game.Random.NextAngle());

                    direction.Normalize();


                    float offset = components.Collider1.Radius + components.Collider2.Radius - distance;
                    transform.Position += direction * offset;
                }
            }

            if (components.Collider1.DestroyOnImpact)
                GameState.DestroyEntity(components.Collider1.EntityIndex);
        }
    }

    private struct CollisionComponents
    {
        public Transform Transform1;
        public Collider Collider1;
        public Transform Transform2;
        public Collider Collider2;
    }
}
