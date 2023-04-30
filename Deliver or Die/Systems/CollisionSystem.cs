using DeliverOrDie.Components;

using HypEcs;

using Microsoft.Xna.Framework;

using System;

namespace DeliverOrDie.Systems;
internal class CollisionSystem : GameSystem<Transform, Collider>
{
    private readonly Query<Transform, Collider, Movement, Health> movementHealthQuery;
    private readonly Query<Transform, Collider, Movement> movementQuery;
    private readonly Query<Transform, Collider, Health> healthQuery;

    public CollisionSystem(GameState gameState)
        : base(gameState)
    {
        movementHealthQuery = gameState.ECSWorld.Query<Transform, Collider, Movement, Health>().Build();
        movementQuery = gameState.ECSWorld.Query<Transform, Collider, Movement>().Not<Health>().Build();
        healthQuery = gameState.ECSWorld.Query<Transform, Collider, Health>().Not<Movement>().Build();
    }

    protected override void Update(ref Transform transform, ref Collider collider)
    {
        Vector2 position = transform.Position;
        float radius = collider.Radius;
        Collider.Layers damageLayer = collider.DamageLayer;
        Collider.Layers collisionLayer = collider.CollisionLayer;
        Collider.Layers reactionLayer = collider.ReactionLayer;
        float damage = collider.Damage;
        Action<object> reaction = collider.Reaction;
        object data = collider.Data;

        movementHealthQuery.Run((count, transforms, colliders, movements, healths) =>
        {
            HandleCollisions(position, radius, transforms, colliders, (i) =>
            {
                if ((reactionLayer & colliders[i].Layer) > 0)
                    reaction?.Invoke(data);
                if ((collisionLayer & colliders[i].Layer) > 0)
                    RevertMovement(position, radius, colliders[i].Radius, movements[i].Speed, ref transforms[i]);
                if ((damageLayer & colliders[i].Layer) > 0)
                    DealDamage(damage, ref transforms[i], ref healths[i]);
            });
        });

        movementQuery.Run((count, transforms, colliders, movements) =>
        {
            HandleCollisions(position, radius, transforms, colliders, (i) =>
            {
                if ((reactionLayer & colliders[i].Layer) > 0)
                    reaction?.Invoke(data);
                if ((collisionLayer & colliders[i].Layer) > 0)
                    RevertMovement(position, radius, colliders[i].Radius, movements[i].Speed, ref transforms[i]);
            });
        });

        healthQuery.Run((count, transforms, colliders, healths) =>
        {
            HandleCollisions(position, radius, transforms, colliders, (i) =>
            {
                if ((reactionLayer & colliders[i].Layer) > 0)
                    reaction?.Invoke(data);
                if ((damageLayer & colliders[i].Layer) > 0)
                    DealDamage(damage, ref transforms[i], ref healths[i]);
            });
        });
    }

    private static void HandleCollisions
    (
        Vector2 position,
        float radius,
        Transform[] transforms,
        Collider[] colliders,
        Action<int> action
    )
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            float distance = Vector2.Distance(position, transforms[i].Position);

            if (distance < radius + colliders[i].Radius)
                action.Invoke(i);
        }
    }

    private static void RevertMovement(Vector2 position1, float radius1, float radius2, float speed2, ref Transform transform2)
    {
        if (speed2 == 0.0f)
            return;

        Vector2 direction = transform2.Position - position1;
        direction.Normalize();

        float distance = radius1 + radius2 - Vector2.Distance(transform2.Position, position1);

        transform2.Position += direction * distance;
    }
    
    private void DealDamage(float damage, ref Transform transform, ref Health health)
    {
        health.Current -= damage;

        if (health.Current < 0)
        {
            health.OnDead?.Invoke(transform.Position);
            GameState.DestroyEntity(health.EntityIndex);
        }
    }
}
